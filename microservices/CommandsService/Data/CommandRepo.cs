using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data;

public class CommandRepo : ICommandRepo
{
    private readonly AppDbContext context;

    public CommandRepo(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Platform>> GetAllPlatformsAsync()
    {
        return await context.Platforms.ToListAsync();
    }
    public void CreatePlatform(Platform platform)
    {
        context.Platforms.Add(platform);
    }
    public async Task<bool> IsPlatformExistsAsync(int platformId)
    {
        return await context.Platforms.AnyAsync(p => p.Id == platformId);
    }
    public async Task<bool> IsExternalPlatformExistsAsync(int platformExternalId)
    {
        return await context.Platforms.AnyAsync(p => p.ExternalId == platformExternalId);
    }

    public async Task<List<Command>> GetCommandsForPlatformAsync(int platformId)
    {
        return await context.Commands.Where(c => c.PlatformId == platformId).ToListAsync();
    }
    public async Task<Command?> GetCommandAsync(int platformId, int commandId)
    {
        return await context.Commands.FirstOrDefaultAsync(c => c.Id == commandId && c.PlatformId == platformId);
    }
    public void CreateCommand(int platformId, Command command)
    {
        command.PlatformId = platformId;
        context.Commands.Add(command);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await context.SaveChangesAsync()) >= 0;
    }
}