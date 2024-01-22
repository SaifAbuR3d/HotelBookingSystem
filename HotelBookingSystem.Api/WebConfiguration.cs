using Asp.Versioning;
using HotelBookingSystem.Api.Filters;
using HotelBookingSystem.Api.Middlewares;
using HotelBookingSystem.Api.Services;
using HotelBookingSystem.Application;
using HotelBookingSystem.Application.Abstractions;
using HotelBookingSystem.Application.Identity;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json.Serialization;
namespace HotelBookingSystem.Api;

/// <summary>
/// Register services in the DI container
/// </summary>
public static class WebConfiguration
{
    /// <summary>
    /// Register services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddWebComponents(
               this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiVersioning(setup =>
        {
            setup.DefaultApiVersion = new ApiVersion(1, 0);
            setup.AssumeDefaultVersionWhenUnspecified = true;
            setup.ReportApiVersions = true;
        }).AddMvc();

        services
        .AddControllers(option =>
            {
                option.Filters.Add<LogActivityFilter>();
            })
        .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
        .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            });

        services.AddEndpointsApiExplorer();

        services.AddProblemDetails()
                .AddExceptionHandler<GlobalExceptionHandler>();

        services.AddSwagger();
        services.AddDateOnlyTimeOnlyStringConverters();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.GuestOnly, policy => policy
                                              .RequireRole(UserRoles.Guest)
                                              .RequireClaim(ClaimTypes.NameIdentifier)
                                              .RequireClaim(ClaimTypes.Email));

            options.AddPolicy(Policies.AdminOnly, policy => policy
                                              .RequireRole(UserRoles.Admin)
                                              .RequireClaim(ClaimTypes.NameIdentifier)
                                              .RequireClaim(ClaimTypes.Email));
        });


        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddApplicationInsightsTelemetry(opt => opt.ConnectionString = configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

        return services;
    }

    private static IServiceCollection AddSwagger(
              this IServiceCollection services)
        => services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Hotel Booking System API",
                Version = "v1",
                Description = "API endpoints for managing hotel bookings",
            });

            setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            setup.UseDateOnlyTimeOnlyStringConverters();

            #region include xml comments
            var actionMethodsXmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var actionMethodsXmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, actionMethodsXmlCommentsFile);

            var DTOsXmlCommentsFile = $"{Assembly.GetAssembly(typeof(ApplicationConfiguration))!.GetName().Name}.xml";
            var DTOsXmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, DTOsXmlCommentsFile);

            setup.IncludeXmlComments(actionMethodsXmlCommentsFullPath);
            setup.IncludeXmlComments(DTOsXmlCommentsFullPath);
            #endregion
        });

}
