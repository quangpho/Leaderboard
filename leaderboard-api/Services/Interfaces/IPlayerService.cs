using LeaderboardApi.Models.DTOs;
namespace LeaderboardApi.Services.Interfaces
{
    public interface IPlayerService
    {
        Task<(PlayerDto playerDto, bool isNew)> SubmitScore(int playerId, int score,CancellationToken cancellationToken);
        Task<List<PlayerDto>> GetTopPlayersAsync(CancellationToken cancellationToken);
        Task<List<PlayerDto>> GetRelativePlayersAsync(int playerId,CancellationToken cancellationToken);
        Task<PlayerDto> GetPlayerAsync(int playerId,CancellationToken cancellationToken);
        Task ResetLeaderboard(CancellationToken cancellationToken);
        Task ReseedLeaderboard(CancellationToken cancellationToken);
    }
}