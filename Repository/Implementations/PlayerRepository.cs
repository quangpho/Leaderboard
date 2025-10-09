using Database;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Interfaces;
namespace Repository.Implementations
{
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        public PlayerRepository(LeaderboardDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly LeaderboardDbContext _dbContext;

        public async Task<List<Player>> GetMultiplePlayers(int skip, int take)
        {
            return await DbSet.OrderByDescending(player => player.Score).Skip(skip).Take(take).ToListAsync();
        }
        public async Task<int> GetPlayerRank(int playerId)
        {
            var row = (await _dbContext.Database
                    .SqlQueryRaw<int>("EXEC GetPlayerRank @PlayerId = {0}", playerId)
                    .ToListAsync())
                .FirstOrDefault();
            return row;
        }
        
        public async Task ReseedLeaderboard()
        {
            var players = new List<Player>()
            {
                new Player
                {
                    Id = 1,
                    Score = 1200,
                    LastSubmitDate = new DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 2,
                    Score = 900,
                    LastSubmitDate = new DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 3,
                    Score = 1200,
                    LastSubmitDate = new DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 4,
                    Score = 1450,
                    LastSubmitDate = new DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 5,
                    Score = 1500,
                    LastSubmitDate = new DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 6,
                    Score = 1700,
                    LastSubmitDate = new DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 7,
                    Score = 450,
                    LastSubmitDate = new DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 8,
                    Score = 980,
                    LastSubmitDate = new DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 9,
                    Score = 1010,
                    LastSubmitDate = new DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                }
            };
            await DbSet.AddRangeAsync(players);
            await _dbContext.SaveChangesAsync();
        }
    }
}