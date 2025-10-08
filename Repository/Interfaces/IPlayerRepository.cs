using Model;
namespace Repository.Interfaces
{
    public interface IPlayerRepository : IRepository<Player>
    {
        Task<List<Player>> GetMultiplePlayers(int skip, int take);
        Task<int> GetPlayerRank(int score);
    }
}