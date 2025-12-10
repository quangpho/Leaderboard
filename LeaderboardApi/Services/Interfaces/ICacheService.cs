namespace LeaderboardApi.Services.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetOrDefault<T>(string key);
        Task Set<T>(string key, T value, TimeSpan? expiration = null);
        Task Remove(string key);
    }
}