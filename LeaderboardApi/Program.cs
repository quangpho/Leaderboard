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