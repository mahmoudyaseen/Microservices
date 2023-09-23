using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data;

public class AppDbContext : DbContext
{
    public required DbSet<Platform> Platforms { get; set; }
    public required DbSet<Command> Commands { get; set; }

    public AppDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Platform>()
            .HasMany(p => p.Commands)
            .WithOne(c => c.Platform!)
            .HasForeignKey(c => c.PlatformId);

        // modelBuilder
        //     .Entity<Command>()
        //     .HasOne(c => c.Platform!)
        //     .WithMany(p => p.Commands)
        //     .HasForeignKey(c => c.PlatformId);

        base.OnModelCreating(modelBuilder);
    }
}