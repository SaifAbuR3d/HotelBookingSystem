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

        logger.LogError("Request Failure {@ErrorType}, {@ErrorMessage}, {@DateTimeUtc}",
                           exception.GetType().Name, exception.Message, DateTime.UtcNow);

        var (statusCode, title, detail) = MapException(exception);

        await Results.Problem(
            title: title,
            statusCode: statusCode,
            detail: detail,
            extensions: new Dictionary<string, object?>
            {
            //["errors"] = new Dictionary<string, string[]>
            //{
            //    [exception.GetType().Name] = [exception.Message]
            //},
               ["traceId"] = traceId
            }

            ).ExecuteAsync(httpContext);

        return true;
    }

    private static (int statusCode, string title, string details/*, Type t (to simulate FluentValidation error response) */) MapException(Exception exception)
    {
        return exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Not Found", exception.Message),
            UnavailableRoomException => (StatusCodes.Status400BadRequest, "Unavailable Room", exception.Message),
            InvalidNumberOfGuestsException => (StatusCodes.Status400BadRequest, "Invalid Number Of Guests", exception.Message),
            BadFileException => (StatusCodes.Status400BadRequest, "Bad File", exception.Message),
            BadRequestException => (StatusCodes.Status400BadRequest, "Bad Request", exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Something went wrong", "We made a mistake but we are working on it")
        };
    }
}
