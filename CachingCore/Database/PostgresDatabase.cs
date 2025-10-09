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
    }
}