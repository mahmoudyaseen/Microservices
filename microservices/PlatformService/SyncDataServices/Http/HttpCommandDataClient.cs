using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;

    public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.configuration = configuration;
    }

    public async Task SendPlatformToCommand(PlatformReadDto platformReadDto)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(platformReadDto), Encoding.UTF8, "application/json");

        var commandServiceUri = $"{configuration["CommandService"]}";
        var response = await httpClient.PostAsync(commandServiceUri, content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("===> Sync Post to commands service was ok.");
        }
        else
        {
            Console.WriteLine("===> Sync Post to commands service was not ok.");
        }
    }
}