using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LeaderboardApi.Models.Entities
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime LastSubmitDate { get; set; }
    }
}
