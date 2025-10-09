using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model;
using Repository.Interfaces;
using Services.Interfaces;
namespace Services.Implementations;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<PlayerService> _logger;

    private readonly int _topPlayerCount;
    private readonly int _relativePlayerCount;

    private const string TopPlayersCacheKey = "TopPlayers";
    public PlayerService(IPlayerRepository playerRepository, IConfiguration configuration, ILogger<PlayerService> logger, ICacheService cacheService)
    {
        _playerRepository = playerRepository;
        _logger = logger;
        _cacheService = cacheService;
        _topPlayerCount = int.TryParse(configuration["PlayerSettings:TopPlayerLimit"], out var topLimit) ? topLimit : 10;
        _relativePlayerCount = int.TryParse(configuration["PlayerSettings:RelativePlayerLimit"], out var relativeLimit) ? relativeLimit : 5;
    }
    public async Task<List<PlayerDto>> GetTopPlayersAsync()
    {
        try
        {
            var cacheTopPlayers = _cacheService.GetOrDefault<List<PlayerDto>>(TopPlayersCacheKey);
            if (cacheTopPlayers != default)
            {
                return cacheTopPlayers;
            }
            var topPlayers = await _playerRepository.GetMultiplePlayers(0, _topPlayerCount);
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
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }

    }
    public async Task<List<PlayerDto>> GetRelativePlayersAsync(PlayerDto playerDto)
    {
        try
        {
            var playerId = int.TryParse(playerDto.PlayerId, out var id) ? id : 0;
            // Get relative players with player in the middle
            var relativePlayers = await _playerRepository.GetMultiplePlayers(Math.Max(0, playerDto.Rank - _relativePlayerCount - 1), _relativePlayerCount * 2 + 1);
            var result = new List<PlayerDto>();
            var playerIndex = relativePlayers.FindIndex(p => p.Id == playerId);

            for (int i = 0; i < relativePlayers.Count; i++)
            {
                if (i == playerIndex)
                {
                    continue;
                }

                result.Add(new PlayerDto()
                {
                    PlayerId = relativePlayers[i].Id.ToString(),
                    Score = relativePlayers[i].Score,
                    Rank = GetRelativeRankFromPlayer(playerDto.Rank, i, playerIndex),
                    LastSubmitDate = relativePlayers[i].LastSubmitDate
                });
            }
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
    public async Task<PlayerDto> GetPlayerAsync(int playerId)
    {
        try
        {
            var player = await _playerRepository.GetByIdAsync(playerId);
            if (player == null)
            {
                throw new ArgumentException($"Failed to get player {playerId}");
            }

            return new PlayerDto
            {
                PlayerId = player.Id.ToString(),
                Score = player.Score,
                Rank = await _playerRepository.GetPlayerRank(playerId),
                LastSubmitDate = player.LastSubmitDate
            };
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e, "Failed to get player {PlayerId}", playerId);
            throw;
        }
    }
    public async Task ResetLeaderboard()
    {
        _cacheService.Remove(TopPlayersCacheKey);
        await _playerRepository.DeleteAllRecords();
    }
    public async Task ReseedLeaderboard()
    {
        await _playerRepository.ReseedLeaderboard();
    }

    public async Task<PlayerDto> SubmitScore(int playerId, int score)
    {
        try
        {
            Player targetPlayer = await _playerRepository.GetByIdAsync(playerId);
            if (targetPlayer == null)
            {
                await _playerRepository.AddAsync(new Player()
                {
                    Id = playerId,
                    Score = score,
                    LastSubmitDate = DateTime.UtcNow
                });
            }
            else if (targetPlayer.Score != score)
            {
                targetPlayer.Score = score;
                targetPlayer.LastSubmitDate = DateTime.UtcNow;
                await _playerRepository.UpdateAsync(targetPlayer);
            }

            var result = new PlayerDto()
            {
                PlayerId = playerId.ToString(),
                Score = score,
                Rank = await _playerRepository.GetPlayerRank(playerId)
            };

            if (result.Rank <= _topPlayerCount)
            {
                _cacheService.Remove(TopPlayersCacheKey);
            }
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
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