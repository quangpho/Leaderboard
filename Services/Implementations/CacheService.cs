using Microsoft.Extensions.Caching.Memory;
using Services.Interfaces;
namespace Services.Implementations
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T GetOrDefault<T>(string key)
        {
            return _cache.TryGetValue(key, out T value) ? value : default!;
        }
        public void Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new MemoryCacheEntryOptions();
            if (expiration.HasValue)
                options.AbsoluteExpirationRelativeToNow = expiration;
            _cache.Set(key, value, options);
        }
        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}