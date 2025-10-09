using CachingCore.Database;
using FastEndpoints;

namespace CachingCore.Modules.CacheTest;

public class CacheSeedResponse
{
    public string Message { get; set; }
}

public class CacheSeedEnpoint(PostgresDatabase database) : EndpointWithoutRequest<CacheSeedResponse>
{
    public override void Configure()
    {
        Get("seed-data");
        Summary(s =>
        {
            s.Summary = "Seed data";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var caches = new List<Cache>();
        for (int i = 1; i <= 10000; i++)
        {
            caches.Add(new Cache
            {
                Description = $"Seeded cache entry #{i}",
                CreatedOn = DateTimeOffset.UtcNow
            });
        }

        await database.Caches.AddRangeAsync(caches);
        await database.SaveChangesAsync();
        
        await Send.OkAsync(cancellation: ct);
    }
}