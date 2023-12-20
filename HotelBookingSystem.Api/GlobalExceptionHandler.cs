using HotelBookingSystem.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

namespace HotelBookingSystem.Api;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        logger.LogError(
            exception,
            "Could not process a request on machine {MachineName}. TraceId: {TraceId}",
            Environment.MachineName,
            traceId);

        var (statusCode, title, detail) = MapException(exception);

        await Results.Problem(
            title: title,
            statusCode: statusCode,
            detail: detail,
            extensions: new Dictionary<string, object?>
            {
                ["traceId"] = traceId
            }
            ).ExecuteAsync(httpContext);

        return true;
    }

    private static (int statusCode, string title, string details) MapException(Exception exception)
    {
        return exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Not Found", exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Something went wrong", "We made a mistake but we are working on it")
        };
    }
}
