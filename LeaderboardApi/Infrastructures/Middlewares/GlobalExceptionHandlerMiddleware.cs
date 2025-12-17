using LeaderboardApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
namespace LeaderboardApi.Infrastructures.Middlewares
{
    public class GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception occurred");
                var problemDetails = CreateProblemDetails(ex, context);
                
                context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, _jsonOptions));
            }
        }

        private ProblemDetails CreateProblemDetails(Exception ex, HttpContext context)
        {
            if (ex is PlayerNotFoundException)
            {
                return new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Player not found",
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };
            }

            return new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred",
                Instance = context.Request.Path
            };
        }
    }
}