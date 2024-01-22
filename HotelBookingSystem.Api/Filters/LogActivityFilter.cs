using HotelBookingSystem.Api.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace HotelBookingSystem.Api.Filters
{
    /// <summary>
    /// Filter to log requests to the API
    /// </summary>
    /// <param name = "logger" ></param>
    public class LogActivityFilter(ILogger<LogActivityFilter> logger) : IAsyncActionFilter
    {

        /// <summary>
        /// log the action name and action arguments and controller name before and after the action is executed
        /// </summary>
        /// <param name = "context" ></param >
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // don't log arguments sent the identity controller
            if (context.Controller.GetType() == typeof(IdentityController))
            {
                logger.LogInformation("Executing {$ActionMethodName} on Controller {$ControllerName}",
                    context.ActionDescriptor.DisplayName,
                    context.Controller);

                await next();

                logger.LogInformation("Action {$ActionMethodName} Finished Execution on Controller {$ControllerName}",
                    context.ActionDescriptor.DisplayName,
                    context.Controller);

                return;
            }

            logger.LogInformation("Executing {$ActionMethodName} on Controller {$ControllerName}, with Arguments {@ActionArguments}",
                context.ActionDescriptor.DisplayName,
                context.Controller,
                JsonSerializer.Serialize(context.ActionArguments));

            await next();

            logger.LogInformation("Action {$ActionMethodName} Finished Execution on Controller {$ControllerName}, with Arguments {@ActionArguments}",
                context.ActionDescriptor.DisplayName,
                context.Controller,
                JsonSerializer.Serialize(context.ActionArguments));
        }
    }
}
