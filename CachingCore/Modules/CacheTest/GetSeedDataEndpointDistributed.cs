using CachingCore.Common;
using CachingCore.Database;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CachingCore.Modules.CacheTest;

public class GetSeedDataEndpointDistributed(RedisCacheService redis, PostgresDatabase database) : EndpointWithoutRequest<DefaultResponse<Cache>>
{
    public override void Configure()
    {
        Get("seeded-data/in-distributed");
        Summary(s =>
        {
            s.Summary = "Get data, distributed caching";
        });
        AllowAnonymous();
    }

    public override async Task<DefaultResponse<Cache>> ExecuteAsync(CancellationToken ct)
    {
        var key = "seeded-data";
        var cachedData = await redis.GetAsync<Cache>(key);
        if (cachedData == null)
            return new DefaultResponse<Cache>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Fetched Data from Redis",
                Data = new List<Cache>
                {
                    cachedData!
                },
                Errors = null
            };

        var dbData = database.Caches
            .AsNoTracking()
            .Take(1000)
            .ToList();
        
        await redis.SetAsync(key, cachedData!, TimeSpan.FromMinutes(5));
        
            return new DefaultResponse<Cache>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Fetched Data from Database",
                Data = dbData!,
                Errors = null
            };
    }
}