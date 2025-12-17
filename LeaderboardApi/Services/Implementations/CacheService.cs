using LeaderboardApi.Services.Interfaces;
using StackExchange.Redis;
using System.Text.Json;
namespace LeaderboardApi.Services.Implementations
{
    public class CacheService : ICacheService
    {
        public CacheService(ConnectionMultiplexer multiplexer)
        {
            _database = multiplexer.GetDatabase();
        }
        
        private readonly IDatabase _database;
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions();
        
        public async Task<T> GetOrDefault<T>(string key)
        {
            var data = await _database.StringGetAsync(key);
            if (data.HasValue)
            {
                return JsonSerializer.Deserialize<T>(data, Options);
            }
            return default;
        }
        public async Task Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            var jsonValue = JsonSerializer.Serialize(value, Options);
            await _database.StringSetAsync(key, jsonValue).ConfigureAwait(false);
        }
        public async Task Remove(string key)
        {
            await _database.KeyDeleteAsync(key).ConfigureAwait(false);
        }
    }
}