using LeaderboardApi.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace LeaderboardApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class LeaderboardController(IPlayerService playerService) : ControllerBase
    {

        [HttpGet]
        [Route("leaderboard/{playerId}")]
        public async  Task<IActionResult> Leaderboard(int playerId)
        {
            try
            {
                var topPlayers = await playerService.GetTopPlayersAsync();
                var relativePlayers = await playerService.GetRelativePlayersAsync(playerId);
                return Ok(new LeaderboardResponseModel()
                {
                    NearbyScores = relativePlayers,
                    TopScores = topPlayers
                });
            }
            catch (Exception)
            {
                return Problem("Internal server error");
            }
        }
        
        [HttpPost]
        [Route("submit")]
        public async Task<IActionResult> Submit(PlayerRequestModel request)
        {
            try
            {
                var player = await playerService.SubmitScore(request.PlayerId, request.Score);
                return Ok(new PlayerResponseModel()
                {
                    Score = player.Score,
                    Rank = player.Rank
                });
            }
            catch (Exception)
            {
                return Problem("Internal server error");
            }
        }

    }
}
