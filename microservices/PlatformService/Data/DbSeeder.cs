using Microsoft.EntityFrameworkCore;
using PlatformService.Model;

namespace PlatformService.Data;

public static class DbSeeder
{
    public static void SeedInitialData(this ModelBuilder modleBuilder)
    {
        modleBuilder.Entity<Platform>().HasData(
            new { Id = 1, Name = ".net", Publisher = "Microsoft", Cost = "Free" },
            new { Id = 2, Name = "Sql Server Express", Publisher = "Microsoft", Cost = "Free" },
            new { Id = 3, Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
        );
    }
}