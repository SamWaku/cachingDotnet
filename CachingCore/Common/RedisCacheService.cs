using StackExchange.Redis;

namespace CachingCore.Common;

public class RedisCacheService
{
    private readonly IDatabase db;

    public  RedisCacheService(IConnectionMultiplexer redis)
    {
        db = redis.GetDatabase();
    }

    public async Task SetAsync()
    {
        return ;
    }

    public async Task GetAsync()
    {
        
    }

    public async Task RemoveAsync()
    {
        
    }
}