using CachingCore.Common;
using CachingCore.Database;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

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
                StatusCode = 0,
                Message = null,
                Data = new List<Cache>
                {
                    cachedData!
                },
                Errors = null
            };
        
        
        
        return new DefaultResponse
        {
            StatusCode = 0,
            Message = null,
            Data = new List<Cache>
            {
                Id = 0,
                Description = null,
                CreatedOn = default
            },
            Errors = null
        };
    }
}