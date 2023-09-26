using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data;

public static class DbSeeder
{
    public static async Task SeedDbAsync(this IApplicationBuilder applicationBuilder)
    {
        Console.WriteLine("===> Seeding Data to Database.");

        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

        var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>()!;
        var platfroms = grpcClient.ReturnAllPlatforms();

        var commandRepo = serviceScope.ServiceProvider.GetService<ICommandRepo>()!;
        foreach (var platform in platfroms) {
            var isPlatformExist = await commandRepo.IsExternalPlatformExistsAsync(platform.ExternalId);
            if (!isPlatformExist)
                commandRepo.CreatePlatform(platform);
        }
        await commandRepo.SaveChangesAsync();
    }
}