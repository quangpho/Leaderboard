using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class LeaderboardDbContext : DbContext
    {
        public LeaderboardDbContext(DbContextOptions<LeaderboardDbContext> options) : base(options)
        {
        }
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                            .HasIndex(p => p.Score);

            base.OnModelCreating(modelBuilder);
        }
    }
}
