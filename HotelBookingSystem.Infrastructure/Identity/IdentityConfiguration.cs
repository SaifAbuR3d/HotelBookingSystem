using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.IdentityInterfaces;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HotelBookingSystem.Infrastructure.Identity;

public static class IdentityConfiguration
{
    public static IServiceCollection AddIdentityInfrastructure(
               this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        var key = configuration.GetSection(nameof(JwtSettings)).GetSection("Key").Value 
            ?? throw new TokenGenerationFailedException("secret key is missing");
        var keyBytes = Encoding.ASCII.GetBytes(key); 

        var issuer = configuration.GetSection(nameof(JwtSettings)).GetSection("Issuer").Value;

        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearer =>
            {
                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
            });

        services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddTransient<IIdentityManager, IdentityManager>();

        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        return services;
    }
}
