using Microsoft.Extensions.Caching.Memory;

namespace CachingCore.Common;

public class Caching : IMemoryCache
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ICacheEntry CreateEntry(object key)
    {
        throw new NotImplementedException();
    }

    public void Remove(object key)
    {
        throw new NotImplementedException();
    }

    public bool TryGetValue(object key, out object? value)
    {
        throw new NotImplementedException();
    }
}