
using System.Text;
using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandsService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration configuration;
    private readonly IEventProcessor eventProcessor;
    private IConnection connection;
    private IModel channel;
    private string queueName;

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        this.configuration = configuration;
        this.eventProcessor = eventProcessor;
        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ()
    {
        var host = configuration["RabbitMQ:Host"];
        var port = int.Parse(configuration["RabbitMQ:Port"]!);
        var factory = new ConnectionFactory()
        {
            HostName = host,
            Port = port
        };

        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange: "Trigger", type: ExchangeType.Fanout);
        queueName = channel.QueueDeclare().QueueName;
        channel.QueueBind(
            queue: queueName,
            exchange: "Trigger",
            routingKey: "");

        Console.WriteLine("==> Listening to a message bus...");

        connection.ConnectionShutdown += RabbitMQ_ConnectionSutdown;
    }

    private void RabbitMQ_ConnectionSutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("==> message bus connection shutdowm");
    }

    public override void Dispose()
    {
        if (connection.IsOpen)
        {
            connection.Close();
            channel.Close();
        }
        base.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (ModuleHandle, ea) =>
        {
            Console.WriteLine("==> Event Recieved.");

            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

            await eventProcessor.ProcessEventAsync(notificationMessage);
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}