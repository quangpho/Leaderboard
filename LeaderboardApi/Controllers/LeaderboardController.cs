using Microsoft.AspNetCore.Mvc;

namespace LeaderboardApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILogger<LeaderboardController> _logger;

        public LeaderboardController(ILogger<LeaderboardController> logger)
        {
            _logger = logger;
        }

    }
}
