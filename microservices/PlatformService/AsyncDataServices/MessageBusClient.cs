using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration configuration;
    private readonly IConnection connection;
    private IModel channel;

    public MessageBusClient(IConfiguration configuration)
    {
        this.configuration = configuration;
        var host = configuration["RabbitMQ:Host"];
        var port = int.Parse(configuration["RabbitMQ:Port"]!);
        var factory = new ConnectionFactory()
        {
            HostName = host,
            Port = port,
        };

        try
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "Trigger", type: ExchangeType.Fanout);

            connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            Console.WriteLine("==> Connected to RabbitMq");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Can't connect to the message bus, {ex.Message}");
            throw;
        }
    }
    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("==> RqbbitMq Connection Shutdown");
    }

    public void PublishNewPlatform(PlatformPublishDto publishDto)
    {
        var message = JsonSerializer.Serialize(publishDto);
        if (connection.IsOpen)
        {
            Console.WriteLine("--> RabbitMQ Connection Open, Sending Message...");
            SendMessage(message);
        }
        else
        {
            Console.WriteLine("--> RabbitMQ Connection closed, Not Sending Message...");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(
            exchange: "Trigger",
            routingKey: "",
            basicProperties: null,
            body: body);

        Console.WriteLine($"--> we have sent {message}");
    }

    public void Dispose()
    {
        Console.WriteLine("Message bus disposed.");
        if (channel.IsOpen)
        {
            channel.Close();
            connection.Close();
        }
    }
}