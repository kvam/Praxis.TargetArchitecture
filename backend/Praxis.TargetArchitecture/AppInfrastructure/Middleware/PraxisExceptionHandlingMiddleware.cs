using System.Net;
using System.Text.Json;
using Praxis.TargetArchitecture.AppInfrastructure.Exceptions;

namespace Praxis.TargetArchitecture.AppInfrastructure.Middleware;

public class PraxisExceptionHandlingMiddleware(
    RequestDelegate requestDelegate,
    ILogger<PraxisExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await requestDelegate(context);
        }
        catch (Exception exception)
        {
            await HandleException(context, exception);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "Unhandled exception for {method} {path}", context.Request.Method, context.Request.Path.Value);

        var (statusCode, message) = MapException(exception);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new Dictionary<string, string>
        {
            { "message", message }
        }));
    }

    private static (HttpStatusCode StatusCode, string Message) MapException(Exception exception) =>
        exception switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, exception.Message),
            ForbiddenException => (HttpStatusCode.Forbidden, exception.Message),
            NotFoundException => (HttpStatusCode.NotFound, exception.Message),
            ConflictException => (HttpStatusCode.Conflict, exception.Message),
            ValidationException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };
}
