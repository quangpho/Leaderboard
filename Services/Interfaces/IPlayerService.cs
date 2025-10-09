using Model;
namespace Services.Interfaces
{
    public interface IPlayerService
    {
        Task<PlayerDto> SubmitScore(int playerId, int score);
        Task<List<PlayerDto>> GetTopPlayersAsync();
        Task<List<PlayerDto>> GetRelativePlayersAsync(PlayerDto playerDto);
        Task<PlayerDto> GetPlayerAsync(int playerId);
        Task ResetLeaderboard();
        Task ReseedLeaderboard();
    }
}