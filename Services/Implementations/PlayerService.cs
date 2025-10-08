using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model;
using Repository.Interfaces;
using Services.Interfaces;
namespace Services.Implementations;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<PlayerService> _logger;

    private readonly int _topPlayerCount;
    private readonly int _relativePlayerCount;
    public PlayerService(IPlayerRepository playerRepository, IConfiguration configuration, IMemoryCache memoryCache, ILogger<PlayerService> logger)
    {
        _playerRepository = playerRepository;
        _memoryCache = memoryCache;
        _logger = logger;
        _topPlayerCount = int.TryParse(configuration["PlayerSettings:TopPlayerLimit"], out var topLimit) ? topLimit : 10;
        _relativePlayerCount = int.TryParse(configuration["PlayerSettings:RelativePlayerLimit"], out var relativeLimit) ? relativeLimit : 5;
    }
    public async Task<List<PlayerDto>> GetTopPlayersAsync()
    {
        try
        {
            const string cacheKey = "PlayerService:TopPlayers";
            if (_memoryCache.TryGetValue(cacheKey, out List<PlayerDto> cachedPlayers))
            {
                return cachedPlayers;
            }
            var topPlayers = await _playerRepository.GetMultiplePlayers(0, _topPlayerCount);
            var topPlayersDto = topPlayers.Select(p => new PlayerDto
            {
                PlayerId = p.Id.ToString(),
                Score = p.Score,
                Rank = topPlayers.IndexOf(p) + 1,
                LastSubmitDate = p.LastSubmitDate
            }).ToList();
            _memoryCache.Set(cacheKey, topPlayers);
            return topPlayersDto;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }

    }
    public async Task<List<PlayerDto>> GetRelativePlayersAsync(int playerId)
    {
        try
        {
            var targetPlayer = await _playerRepository.GetByIdAsync(playerId);
            if (targetPlayer == null)
            {
                throw new ArgumentException("Player not found");
            }
            var playerRank = await _playerRepository.GetPlayerRank(targetPlayer.Score);
            var relativePlayers = await _playerRepository.GetMultiplePlayers(Math.Max(0, playerRank - _relativePlayerCount - 1), _relativePlayerCount * 2 + 1);
            var result = new List<PlayerDto>();
            var targetPlayerIndex = relativePlayers.IndexOf(targetPlayer);

            for (int i = 0; i < relativePlayers.Count; i++)
            {
                if (relativePlayers[i].Id == targetPlayer.Id)
                {
                    continue;
                }

                result.Add(new PlayerDto()
                {
                    PlayerId = relativePlayers[i].Id.ToString(),
                    Score = relativePlayers[i].Score,
                    Rank = i < targetPlayerIndex ? playerRank - (_relativePlayerCount - i) : playerRank + (i - targetPlayerIndex)
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
            else
            {
                targetPlayer.Score = score;
                targetPlayer.LastSubmitDate = DateTime.UtcNow;
                await _playerRepository.UpdateAsync(targetPlayer);
            }
            return new PlayerDto()
            {
                PlayerId = playerId.ToString(),
                Score = score,
                Rank = await _playerRepository.GetPlayerRank(score) + 1
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }

    }
    public Task<List<PlayerDto>> GetLeaderboard(int player)
    {
        throw new NotImplementedException();
    }
    public Task ResetLeaderboard()
    {
        throw new NotImplementedException();
    }
}