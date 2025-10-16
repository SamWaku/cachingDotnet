using System.Text.Json;
using StackExchange.Redis;

namespace CachingCore.Common;

public class RedisCacheService
{
    private readonly IDatabase db;

    public  RedisCacheService(IConnectionMultiplexer redis)
    {
        db = redis.GetDatabase();
    }

    public async Task SetAsync<T>(String key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        await db.StringSetAsync(key, json, expiry);
    }

    public async Task<T?> GetAsync<T>(String key)
    {
        var value = await db.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return default;
        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task RemoveAsync(String key)
    {
        await db.KeyDeleteAsync(key);
    }
}