using LeaderboardApi.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
namespace LeaderboardApi.Services.Implementations
{
    // Note: Currently using in-memory caching for simplicity.
    public class CacheService(IMemoryCache cache) : ICacheService
    {
        public T GetOrDefault<T>(string key)
        {
            return cache.TryGetValue(key, out T value) ? value : default!;
        }
        public void Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new MemoryCacheEntryOptions();
            if (expiration.HasValue)
                options.AbsoluteExpirationRelativeToNow = expiration;
            cache.Set(key, value, options);
        }
        public void Remove(string key)
        {
            cache.Remove(key);
        }
    }
}