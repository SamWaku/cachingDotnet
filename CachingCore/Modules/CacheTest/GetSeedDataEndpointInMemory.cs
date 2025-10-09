using CachingCore.Database;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CachingCore.Modules.CacheTest;

public class DefaultResponse<T> where T : class
{
    public required int StatusCode { get; set; }
    public required string Message { get; set; }
    public List<T> Data { get; set; }
    public List<string> Errors { get; set; } = [];
}


public class GetSeedDataEndpointInMemory(PostgresDatabase database, IMemoryCache cache) : EndpointWithoutRequest<DefaultResponse<Cache>>
{
    public override void Configure()
    {
        Get("seeded-data");
        Summary(s =>
        {
            s.Summary = "Get data, no caching";
        });
        AllowAnonymous();
    }

    public override async Task<DefaultResponse<Cache>> ExecuteAsync(CancellationToken ct)
    {
       

        if (!cache.TryGetValue("seededData", out List<Cache> cachedData))
        {
            cachedData = await database.Caches.AsNoTracking().ToListAsync();
            
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));
            
            cache.Set("seededData", cachedData, cacheEntryOptions);
        }
        return new DefaultResponse<Cache>
        { 
            StatusCode = 200,
            Message = "Success",
            Data = cachedData!,
            Errors = null
        };
    }
}