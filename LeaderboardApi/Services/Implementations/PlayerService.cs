using LeaderboardApi.Exceptions;
using LeaderboardApi.Models.DTOs;
using LeaderboardApi.Models.Entities;
using LeaderboardApi.Repository.Interfaces;
using LeaderboardApi.Services.Interfaces;
namespace LeaderboardApi.Services.Implementations;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ICacheService _cacheService;

    private readonly int _topPlayerCount;
    private readonly int _relativePlayerCount;

    private const string TopPlayersCacheKey = "TopPlayers";
    public PlayerService(IPlayerRepository playerRepository, IConfiguration configuration, ICacheService cacheService)
    {
        _playerRepository = playerRepository;
        _cacheService = cacheService;
        _topPlayerCount = int.TryParse(configuration["PlayerSettings:TopPlayerLimit"], out var topLimit) ? topLimit : 10;
        _relativePlayerCount = int.TryParse(configuration["PlayerSettings:RelativePlayerLimit"], out var relativeLimit) ? relativeLimit : 3;
    }
    public async Task<List<PlayerDto>> GetTopPlayersAsync(CancellationToken cancellationToken)
    {
        var cacheTopPlayers = _cacheService.GetOrDefault<List<PlayerDto>>(TopPlayersCacheKey);
        if (cacheTopPlayers != default)
        {
            return cacheTopPlayers;
        }
        var topPlayers = await _playerRepository.GetMultiplePlayers(0, _topPlayerCount, cancellationToken);
        var topPlayersDto = topPlayers.Select((player, index) => new PlayerDto
        {
            PlayerId = player.Id.ToString(),
            Score = player.Score,
            Rank = index + 1,
            LastSubmitDate = player.LastSubmitDate
        }).ToList();
        _cacheService.Set(TopPlayersCacheKey, topPlayersDto, new TimeSpan(7, 0, 0, 0));
        return topPlayersDto;
    }
    public async Task<List<PlayerDto>> GetRelativePlayersAsync(int playerId, CancellationToken cancellationToken)
    {
        // Get relative players with player in the middle
        var relativePlayers = await _playerRepository.GetRelativePlayers(playerId, _relativePlayerCount, cancellationToken);
        return relativePlayers.Where(player => player.Id != playerId)
            .Select((player) => new PlayerDto
            {
                PlayerId = player.Id.ToString(),
                Score = player.Score,
                Rank = (int)player.Rank,
                LastSubmitDate = player.LastSubmitDate
            }).ToList();
    }
    public async Task<PlayerDto> GetPlayerAsync(int playerId, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetByIdAsync(playerId, cancellationToken);
        if (player == null)
        {
            throw new PlayerNotFoundException($"Failed to get player {playerId}");
        }

        return new PlayerDto
        {
            PlayerId = player.Id.ToString(),
            Score = player.Score,
            Rank = await _playerRepository.GetPlayerRank(playerId, cancellationToken),
            LastSubmitDate = player.LastSubmitDate
        };
    }
    public async Task ResetLeaderboard(CancellationToken cancellationToken)
    {
        _cacheService.Remove(TopPlayersCacheKey);
        await _playerRepository.DeleteAllRecords(cancellationToken);
    }
    public async Task ReseedLeaderboard(CancellationToken cancellationToken)
    {
        await _playerRepository.ReseedLeaderboard(cancellationToken);
    }

    public async Task<(PlayerDto playerDto, bool isNew)> SubmitScore(int playerId, int score, CancellationToken cancellationToken)
    {
        var isNew = false;
        Player targetPlayer = await _playerRepository.GetByIdAsync(playerId, cancellationToken);
        if (targetPlayer == null)
        {
            await _playerRepository.AddAsync(new Player()
            {
                Id = playerId,
                Score = score,
                LastSubmitDate = DateTime.UtcNow
            }, cancellationToken);
            isNew = true;
        }
        else if (targetPlayer.Score != score)
        {
            targetPlayer.Score = score;
            targetPlayer.LastSubmitDate = DateTime.UtcNow;
            await _playerRepository.UpdateAsync(targetPlayer, cancellationToken);
        }

        var result = new PlayerDto()
        {
            PlayerId = playerId.ToString(),
            Score = score,
            Rank = await _playerRepository.GetPlayerRank(playerId, cancellationToken)
        };

        if (result.Rank <= _topPlayerCount)
        {
            _cacheService.Remove(TopPlayersCacheKey);
        }
        return (result, isNew);
    }

    private int GetRelativeRankFromPlayer(int playerRank, int currentIndex, int playerIndex)
    {
        int relativeRank;

        if (currentIndex < playerIndex)
        {
            relativeRank = playerRank - (playerIndex - currentIndex);
        }
        else
        {
            relativeRank = playerRank + (currentIndex - playerIndex);
        }

        return Math.Max(1, relativeRank);
    }
}