namespace LeaderboardApi.Exceptions
{
    public class PlayerNotFoundException(string message) : Exception(message);
}