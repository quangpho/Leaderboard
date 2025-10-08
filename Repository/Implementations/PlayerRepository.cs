using Database;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Interfaces;
namespace Repository.Implementations
{
    public class PlayerRepository(LeaderboardDbContext context) : Repository<Player>(context), IPlayerRepository
    {
        public async Task<List<Player>> GetMultiplePlayers(int skip, int take)
        {
            return await DbSet.OrderByDescending(player => player.Score).Skip(skip).Take(take).ToListAsync();
        }
        public async Task<int> GetPlayerRank(int score)
        {
            var quang = DbSet.OrderByDescending(player => player.Score).ToList();
            var quang2 = quang.FindIndex(0, player => player.Id == score);
            return await DbSet.CountAsync(player => player.Score > score);
        }
    }
}