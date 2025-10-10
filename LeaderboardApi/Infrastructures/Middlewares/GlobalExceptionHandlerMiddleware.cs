using LeaderboardApi.Exceptions;
using System.Net;
using System.Text.Json;
namespace LeaderboardApi.Infrastructures.Middlewares
{
    public class GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace);
                int statusCode;
                string message;

                if (ex is PlayerNotFoundException)
                {
                    statusCode = (int) HttpStatusCode.BadRequest;
                    message = ex.Message;
                }
                else
                {
                    statusCode = (int) HttpStatusCode.InternalServerError;
                    message = "Internal server error";
                }
                
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    statusCode,
                    message,
                };

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
            }
        }

    }
}