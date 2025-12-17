using Database;
using LeaderboardApi.Infrastructures;
using LeaderboardApi.Infrastructures.Database;
using LeaderboardApi.Infrastructures.Middlewares;
using LeaderboardApi.Repository.Implementations;
using LeaderboardApi.Repository.Interfaces;
using LeaderboardApi.Services.Implementations;
using LeaderboardApi.Services.Interfaces;
using LeaderboardApi.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

// Add db context
builder.Services.AddDbContext<LeaderboardDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

// Add settings
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection(RedisSettings.ConfigName));

// Add dependency injections
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddSingleton<ConnectionMultiplexer>(sp =>
{
    var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
    return ConnectionMultiplexer.Connect(new ConfigurationOptions
    {
        EndPoints = { redisSettings.ConnectionString },
        User = redisSettings.Username,
        Password = redisSettings.Password
    });
});
builder.Services.AddSingleton<ICacheService, CacheService>();

    
var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();