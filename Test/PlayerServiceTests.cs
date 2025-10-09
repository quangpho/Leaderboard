using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model;
using Moq;
using Repository.Interfaces;
using Services.Implementations;
using Services.Interfaces;
using Xunit;
namespace Test;

public class PlayerServiceTests
{
    private readonly Mock<IPlayerRepository> _mockRepo;
    private readonly Mock<ICacheService> _mockCache;
    private readonly Mock<ILogger<PlayerService>> _mockLogger;
    private readonly IConfiguration _configuration;
    private readonly PlayerService _service;
    
    private const string TopPlayersCacheKey = "TopPlayers";

    public PlayerServiceTests()
    {
        _mockRepo = new Mock<IPlayerRepository>();
        _mockCache = new Mock<ICacheService>();
        _mockLogger = new Mock<ILogger<PlayerService>>();

        var inMemorySettings = new Dictionary<string, string>
        {
            {
                "PlayerSettings:TopPlayerLimit", "10"
            },
            {
                "PlayerSettings:RelativePlayerLimit", "5"
            }
        };
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _service = new PlayerService(_mockRepo.Object, _configuration,_mockLogger.Object, _mockCache.Object);
    }

    [Fact]
    public async Task GetTopPlayersAsync_ReturnsCachedPlayers_IfExist()
    {
        var cachedPlayers = new List<PlayerDto>
        {
            new PlayerDto
            {
                PlayerId = "1",
                Score = 100,
                Rank = 1
            }
        };

        _mockCache.Setup(m => m.GetOrDefault<List<PlayerDto>>(TopPlayersCacheKey)).Returns(cachedPlayers);

        var result = await _service.GetTopPlayersAsync();

        Assert.Single(result);
        Assert.Equal("1", result[0].PlayerId);
        _mockRepo.Verify(r => r.GetMultiplePlayers(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetTopPlayersAsync_FetchesFromRepo_IfCacheMiss()
    {
        _mockCache.Setup(m => m.GetOrDefault<List<PlayerDto>>(TopPlayersCacheKey)).Returns((List<PlayerDto>) default);
    
        var players = new List<Player>
        {
            new Player
            {
                Id = 1,
                Score = 100,
                LastSubmitDate = DateTime.UtcNow
            },
            new Player
            {
                Id = 2,
                Score = 90,
                LastSubmitDate = DateTime.UtcNow
            }
        };
    
        _mockRepo.Setup(r => r.GetMultiplePlayers(0, 10)).ReturnsAsync(players);
    
        var mockCacheSet = new Mock<ICacheEntry>();
    
        var result = await _service.GetTopPlayersAsync();
    
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].Rank);
        _mockRepo.Verify(r => r.GetMultiplePlayers(0, 10), Times.Once);
    }

    [Fact]
    public async Task SubmitScore_AddsNewPlayer_IfNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Player) null);
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<Player>())).Returns(Task.CompletedTask);
        _mockRepo.Setup(r => r.GetPlayerRank(1)).ReturnsAsync(1);

        var result = await _service.SubmitScore(1, 100);

        Assert.Equal(1, result.Rank);
        Assert.Equal(100, result.Score);
        _mockRepo.Verify(r => r.AddAsync(It.Is<Player>(p => p.Id == 1 && p.Score == 100)), Times.Once);
    }

    [Fact]
    public async Task SubmitScore_UpdatesPlayer_IfScoreChanged()
    {
        var existingPlayer = new Player
        {
            Id = 1,
            Score = 50,
            LastSubmitDate = DateTime.UtcNow.AddDays(-1)
        };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingPlayer);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Player>())).Returns(Task.CompletedTask);
        _mockRepo.Setup(r => r.GetPlayerRank(1)).ReturnsAsync(1);

        var result = await _service.SubmitScore(1, 100);

        Assert.Equal(100, existingPlayer.Score);
        _mockRepo.Verify(r => r.UpdateAsync(It.Is<Player>(p => p.Score == 100)), Times.Once);
    }

    [Fact]
    public async Task GetPlayerAsync_ReturnsPlayerDto()
    {
        var player = new Player
        {
            Id = 1,
            Score = 100,
            LastSubmitDate = DateTime.UtcNow
        };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(player);
        _mockRepo.Setup(r => r.GetPlayerRank(1)).ReturnsAsync(1);

        var result = await _service.GetPlayerAsync(1);

        Assert.NotNull(result);
        Assert.Equal("1", result.PlayerId);
        Assert.Equal(100, result.Score);
        Assert.Equal(1, result.Rank);
    }
}