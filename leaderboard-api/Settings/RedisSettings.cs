namespace LeaderboardApi.Settings
{
    public class RedisSettings
    {
        public const string ConfigName = "Redis";
        public string ConnectionString { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}