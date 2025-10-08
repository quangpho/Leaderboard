using Model;
namespace Services.Interfaces
{
    public interface IPlayerService
    {
        Task<PlayerDto> SubmitScore(int playerId, int score);
        Task<List<PlayerDto>> GetTopPlayersAsync();
        Task<List<PlayerDto>> GetRelativePlayersAsync(int playerId);
        Task ResetLeaderboard();
    }
}