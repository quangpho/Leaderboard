using LeaderboardApi.Models.DTOs;
namespace LeaderboardApi.Models.Respones
{
    public class LeaderboardResponseModel
    {
        public List<PlayerDto> TopScores { get; set; }
        public List<PlayerDto> NearbyScores { get; set; }
    }
}