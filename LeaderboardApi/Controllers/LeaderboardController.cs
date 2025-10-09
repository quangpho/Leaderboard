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
        public async Task<IActionResult> Leaderboard(int playerId)
        {
            try
            {
                var targetPlayer = await playerService.GetPlayerAsync(playerId);
                var topPlayers = await playerService.GetTopPlayersAsync();
                var relativePlayers = await playerService.GetRelativePlayersAsync(targetPlayer);

                return Ok(new LeaderboardResponseModel()
                {
                    NearbyScores = relativePlayers,
                    TopScores = topPlayers
                });
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
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
        
        [HttpDelete]
        [Route("reset")]
        public async Task<IActionResult> Reset()
        {
            try
            {
                await playerService.ResetLeaderboard();
                return Ok("Leaderboard reset successfully");
            }
            catch (Exception)
            {
                return Problem("Internal server error");
            }
        }
        
        [HttpGet]
        [Route("reseed")]
        public async Task<IActionResult> Reseed()
        {
            try
            {
                await playerService.ReseedLeaderboard();
                return Ok("Leaderboard reseed successfully");
            }
            catch (Exception)
            {
                return Problem("Internal server error");
            }
        }
    }
}