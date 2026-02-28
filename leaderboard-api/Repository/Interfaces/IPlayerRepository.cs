using LeaderboardApi.Models.Entities;
using LeaderboardApi.Models.Results;
namespace LeaderboardApi.Repository.Interfaces
{
    public interface IPlayerRepository : IRepository<Player>
    {
        Task<List<Player>> GetMultiplePlayers(int skip, int take, CancellationToken cancellationToken);
        Task<int> GetPlayerRank(int playerId, CancellationToken cancellationToken);
        Task ReseedLeaderboard(CancellationToken cancellationToken);
        Task<List<PlayerQueryResult>> GetRelativePlayers(int playerId, int relativeCount ,CancellationToken cancellationToken);
    }
}