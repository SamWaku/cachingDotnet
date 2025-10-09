using CachingCore.Modules.CacheTest;
using Microsoft.EntityFrameworkCore;

namespace CachingCore.Database;

public class PostgresDatabase(DbContextOptions<PostgresDatabase> options) : DbContext(options)
{
    public DbSet<Cache> Caches { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("public");
        base.OnModelCreating(builder);
        var cacheSeed = new List<Cache>();
        for (int i = 1; i <= 10000; i++)
        {
            cacheSeed.Add(new Cache
            {
                Id = i,
                Description = $"Seeded cache entry #{i}",
                CreatedOn = DateTimeOffset.UtcNow
            });
        }

        builder.Entity<Cache>().HasData(cacheSeed);
    }
}