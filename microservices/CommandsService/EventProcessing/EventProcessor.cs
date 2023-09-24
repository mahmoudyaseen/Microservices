using System.Text;
using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly IMapper mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        this.scopeFactory = scopeFactory;
        this.mapper = mapper;
    }

    public async Task ProcessEventAsync(string message)
    {
        var eventType = DetermineEvent(message);
        Console.WriteLine($"===> Selected event type is {eventType}");

        switch (eventType)
        {
            case EventType.PlatformPublished:
                await AddPlatformAsync(message);
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine("==> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        return eventType?.Event switch
        {
            "Platform_Published" => EventType.PlatformPublished,
            _ => EventType.Undetermind,
        };
    }

    private async Task AddPlatformAsync(string platformPublishMessage)
    {
        using var scope = scopeFactory.CreateScope();
        var commandRepo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

        var platformPublishDto = JsonSerializer.Deserialize<PlatformPublishDto>(platformPublishMessage);
        var platform = mapper.Map<Platform>(platformPublishDto);
        try
        {
            var isPlatformExists = await commandRepo!.IsExternalPlatformExistsAsync(platform.ExternalId);
            if (!isPlatformExists)
            {
                commandRepo.CreatePlatform(platform);
                await commandRepo.SaveChangesAsync();
                Console.WriteLine($"Platform Added.");
            }
            else
            {
                Console.WriteLine($"Platform already exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't add published platform, {ex.Message}");
            throw;
        }
    }
}
enum EventType
{
    PlatformPublished,
    Undetermind,
}