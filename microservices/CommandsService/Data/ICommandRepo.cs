using CommandsService.Models;

namespace CommandsService.Data;

public interface ICommandRepo
{
    Task<List<Platform>> GetAllPlatformsAsync();
    void CreatePlatform(Platform platform);
    Task<bool> IsPlatformExistsAsync(int platformId);
    Task<bool> IsExternalPlatformExistsAsync(int platformExternalId);

    Task<List<Command>> GetCommandsForPlatformAsync(int platformId);
    Task<Command?> GetCommandAsync(int platformId, int commandId);
    void CreateCommand(int platformId, Command command);

    Task<bool> SaveChangesAsync();
}