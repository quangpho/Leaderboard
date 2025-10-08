using Microsoft.EntityFrameworkCore;
using Model;

namespace Database
{
    public class LeaderboardDbContext(DbContextOptions<LeaderboardDbContext> options) : DbContext(options)
    {
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Score).IsDescending();

            modelBuilder.Entity<Player>().Property(p => p.Id).ValueGeneratedNever();
            modelBuilder.Entity<Player>().HasData(new List<Player>()
            {
                new Player
                {
                    Id = 1,
                    Score = 1200,
                    LastSubmitDate = new  DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 2,
                    Score = 900,
                    LastSubmitDate = new  DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 3,
                    Score = 1200,
                    LastSubmitDate = new  DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 4,
                    Score = 1450,
                    LastSubmitDate = new  DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 5,
                    Score = 1500,
                    LastSubmitDate = new  DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 6,
                    Score = 1700,
                    LastSubmitDate = new  DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 7,
                    Score = 450,
                    LastSubmitDate = new  DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 8,
                    Score = 980,
                    LastSubmitDate = new  DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Player
                {
                    Id = 9,
                    Score = 1010,
                    LastSubmitDate = new  DateTime(2025, 10, 8, 12, 0, 0, DateTimeKind.Utc)
                }
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}