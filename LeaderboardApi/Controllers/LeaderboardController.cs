using LeaderboardApi.Models.Requests;
using LeaderboardApi.Models.Respones;
using LeaderboardApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeaderboardApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class LeaderboardController(IPlayerService playerService) : ControllerBase
    {

        [HttpGet]
        [Route("leaderboard/{playerId}")]
        public async Task<IActionResult> Leaderboard(int playerId, CancellationToken cancellationToken)
        {
            var topPlayers = await playerService.GetTopPlayersAsync(cancellationToken);
            var relativePlayers = await playerService.GetRelativePlayersAsync(playerId, cancellationToken);

            return Ok(new LeaderboardResponseModel()
            {
                NearbyScores = relativePlayers,
                TopScores = topPlayers
            });

        }

        [HttpPost]
        [Route("submit")]
        public async Task<IActionResult> Submit(PlayerRequestModel request, CancellationToken cancellationToken)
        {
            var result = await playerService.SubmitScore(request.PlayerId, request.Score, cancellationToken);
            if (result.isNew)
            {
                return Created($"/api/leaderboard/{result.playerDto.PlayerId}", new PlayerResponseModel()
                {
                    Score = result.playerDto.Score,
                    Rank = result.playerDto.Rank,
                    Status = "Created"
                });
            }

            return Ok(new PlayerResponseModel()
            {
                Score = result.playerDto.Score,
                Rank = result.playerDto.Rank,
                Status = "Updated"
            });
        }

        [HttpDelete]
        [Route("reset")]
        public async Task<IActionResult> Reset(CancellationToken cancellationToken)
        {
            await playerService.ResetLeaderboard(cancellationToken);
            return Ok("Leaderboard reset successfully");
        }

        [HttpGet]
        [Route("reseed")]
        public async Task<IActionResult> Reseed(CancellationToken cancellationToken)
        {
            await playerService.ReseedLeaderboard(cancellationToken);
            return Ok("Leaderboard reseed successfully");
        }
    }
}