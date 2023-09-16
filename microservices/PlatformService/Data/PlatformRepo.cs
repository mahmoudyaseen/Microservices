using Microsoft.EntityFrameworkCore;
using PlatformService.Model;

namespace PlatformService.Data;

public class PlatformRepo : IPlatformRepo
{
    private readonly AppDbContext context;

    public PlatformRepo(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Platform>> GetAllPlatformsAsync()
    {
        return await context.Platforms.ToListAsync();
    }
    public async Task<Platform?> GetPlatformByIdAsync(int id)
    {
        return await context.Platforms.FirstOrDefaultAsync(x => x.Id == id);
    }
    public void CreatePlatform(Platform platform)
    {
        context.AddAsync(platform);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await context.SaveChangesAsync()) >= 0;
    }
}