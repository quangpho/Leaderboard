# Stop script on error
$ErrorActionPreference = "Stop"

Write-Host "Cleaning solution..."
dotnet clean

Write-Host "Restoring dependencies..."
dotnet restore

Write-Host "Building all projects..."
dotnet build --configuration Release

# Optional: Run database migrations (if Database project uses EF Core)
Write-Host "Updating database..."
dotnet ef database update --project LeaderboardApi

Write-Host "Running unit tests..."
dotnet test ./LeaderboardApi/Test --configuration Release --no-build

Write-Host "Starting Leaderboard API..."
dotnet run --project ./LeaderboardApi/LeaderboardApi.csproj --no-build
