using LeaderboardApi.Services.Interfaces;
using LeaderboardApi.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;
namespace LeaderboardApi.Services.Implementations
{
    // Note: Currently using in-memory caching for simplicity.
    public class CacheService(IOptions<RedisSettings> redisSettings) : ICacheService
    {
        private IDatabase Database;
        private readonly RedisSettings _redisSettings = redisSettings.Value;

        public void CreateDatabase()
        {
            var muxer = ConnectionMultiplexer.Connect(
                new ConfigurationOptions
                {
                    EndPoints =
                    {
                        _redisSettings.ConnectionString
                    },
                    User = _redisSettings.Username,
                    Password = _redisSettings.Password
                }
            );
            Database = muxer.GetDatabase();
        }

        public async Task<T> GetOrSet<T>(string key)
        {
            
        }
        public async Task<T> GetOrDefault<T>(string key)
        {
            var data = await Database.StringGetAsync(key);
            if (data.HasValue)
            {
                return JsonSerializer.Deserialize<T>(data);
            }
            return default;
        }
        public async Task Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            var jsonValue = JsonSerializer.Serialize(value);
            await Database.StringGetAsync(key, jsonValue, expiration);
        }
        public async Task Remove(string key)
        {
            await Database.KeyDeleteAsync(key);
        }
    }
}