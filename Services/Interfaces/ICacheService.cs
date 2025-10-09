namespace Services.Interfaces
{
    public interface ICacheService
    {
        T GetOrDefault<T>(string key);
        void Set<T>(string key, T value, TimeSpan? expiration = null);
        void Remove(string key);
    }
}