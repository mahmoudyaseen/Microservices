using PlatformService.Model;

namespace PlatformService.Data;

public interface IPlatformRepo
{
    Task<List<Platform>> GetAllPlatformsAsync();
    Task<Platform?> GetPlatformByIdAsync(int id);
    void CreatePlatform(Platform platform);

    Task<bool> SaveChangesAsync();
}