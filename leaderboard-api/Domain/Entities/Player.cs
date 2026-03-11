using System.ComponentModel.DataAnnotations;
namespace LeaderboardApi.Domain.Entities
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime LastSubmitDate { get; set; }
    }
}
