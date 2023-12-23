using HotelBookingSystem.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

namespace HotelBookingSystem.Api;

/// <summary>
/// Exception Handling Middleware
/// </summary>
/// <param name="logger"></param>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    /// <summary>
    /// Handles the exception
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="exception"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>true if the exception was properly handled</returns>
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
            UnavailableRoomException => (StatusCodes.Status400BadRequest, "Unavailable Room", exception.Message),
            InvalidNumberOfGuestsException => (StatusCodes.Status400BadRequest, "Invalid Number Of Guests", exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Something went wrong", "We made a mistake but we are working on it")
        };
    }
}
