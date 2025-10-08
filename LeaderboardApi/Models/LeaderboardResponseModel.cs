using Model;
namespace LeaderboardApi.Models
{
    public class LeaderboardResponseModel
    {
        public List<PlayerDto> TopScores { get; set; }
        public List<PlayerDto> NearbyScores { get; set; }
    }
}