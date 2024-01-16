using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.EmailInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBookingSystem.Infrastructure.Email;

public static class EmailConfiguration
{
    public static IServiceCollection AddEmailInfrastructure(
               this IServiceCollection services,
                      IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}
