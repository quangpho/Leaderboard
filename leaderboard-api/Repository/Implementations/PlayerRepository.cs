using LeaderboardApi.Infrastructures.Database;
using LeaderboardApi.Models.Entities;
using LeaderboardApi.Models.Results;
using LeaderboardApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace LeaderboardApi.Repository.Implementations
{
    public class PlayerRepository(LeaderboardDbContext dbContext) : Repository<Player>(dbContext), IPlayerRepository
    {
        private readonly LeaderboardDbContext _dbContext = dbContext;

        public async Task<List<Player>> GetMultiplePlayers(int skip, int take, CancellationToken cancellationToken)
        {
            return await DbSet.OrderByDescending(player => player.Score).Skip(skip).Take(take).ToListAsync(cancellationToken);
        }
        public async Task<int> GetPlayerRank(int playerId, CancellationToken cancellationToken)
        {
            var row = (await _dbContext.Database
                    .SqlQueryRaw<int>("EXEC GetPlayerRank @PlayerId = {0}", playerId)
                    .ToListAsync(cancellationToken))
                .FirstOrDefault();
            return row;
        }
        
        public async Task<List<PlayerQueryResult>> GetRelativePlayers(int playerId, int relativeCount, CancellationToken cancellationToken)
        {
            const string sql = "EXEC GetRelativePlayers @PlayerId = {0}, @Range = {1}";

            var relativePlayers = await _dbContext.Database
                .SqlQueryRaw<PlayerQueryResult>(sql, playerId, relativeCount)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return relativePlayers;
        }

        public async Task ReseedLeaderboard(CancellationToken cancellationToken)
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
            await DbSet.AddRangeAsync(players, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}