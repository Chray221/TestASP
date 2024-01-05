using Microsoft.Extensions.Caching.Memory;

namespace TestASP.Web.Services;

public class MyMemoryCache
{
    public MemoryCache Cache { get; } = new MemoryCache(
        new MemoryCacheOptions
        {
            SizeLimit = 1024
        });

    public void Set<T>(string key, T data)
    {
        var cachedData = new MemoryCacheEntryOptions()
            .SetSize(1)
            .SetAbsoluteExpiration(TimeSpan.FromDays(3));
        Cache.Set(key,data, cachedData);
    }

    public void SetPermanent<T>(string key, T data)
    {
        var cachedData = new MemoryCacheEntryOptions()
            .SetSize(1)
            .SetAbsoluteExpiration(TimeSpan.FromDays(3))
            .SetPriority(CacheItemPriority.NeverRemove);
        Cache.Set(key,data, cachedData);
    }

    public T? Get<T>(string key)
    {
        return Cache.Get<T>(key);
    }
}
