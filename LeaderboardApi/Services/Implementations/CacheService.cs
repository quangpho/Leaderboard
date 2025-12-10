using LeaderboardApi.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using System.Text.Json;
namespace LeaderboardApi.Services.Implementations
{
    // Note: Currently using in-memory caching for simplicity.
    public class CacheService : ICacheService
    {
        private IDatabase _database;
        public CacheService(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("Redis:ConnectionString").Value;
            var userName = configuration.GetSection("Redis:Username").Value;
            var password = configuration.GetSection("Redis:Password").Value;
            var muxer = ConnectionMultiplexer.Connect(
                new ConfigurationOptions
                {
                    EndPoints =
                    {
                        connectionString
                    },
                    User = userName,
                    Password = password
                }
            );
            _database = muxer.GetDatabase();
        }

        public async Task<T> GetOrDefault<T>(string key)
        {
            var data = await _database.StringGetAsync(key);
            if (data.HasValue)
            {
                return JsonSerializer.Deserialize<T>(data);
            }
            return default;
        }
        public async Task Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            var jsonValue = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, jsonValue, expiration);
        }
        public async Task Remove(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}