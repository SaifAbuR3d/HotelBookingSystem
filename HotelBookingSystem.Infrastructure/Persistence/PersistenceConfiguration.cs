using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBookingSystem.Infrastructure.Persistence;

public static class PersistenceConfiguration
{
    public static IServiceCollection AddPersistenceInfrastructure(
        this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        => services
            .AddDatabase(GetConnectionString(configuration), isDevelopment)
            .AddRepositories()
            .AddImageHandling();
    private static string GetConnectionString(IConfiguration configuration)
    {
        var server = configuration["DBServer"] ?? "sql_server2022";
        var database = configuration["Database"] ?? "HotelBookingSystem";
        var user = configuration["DBUser"] ?? "sa";

        var password = configuration["DBPassword"];  // this should be in Azure Key Vault
                                                     // or provided by an environment variable (with docker compose)
      

       
        if (string.IsNullOrWhiteSpace(password))
        {
            // if we are here, we are running locally and we need to use the connection string (from appsettings.json) for SQL Server
            return configuration.GetConnectionString("SqlServer");
        }

        // if we are here, we are running in a container and we need to use the connection string for SQL Server

        Console.WriteLine("Running in a container");
        Console.WriteLine("Waiting for SQL Server to be ready...");

        // wait for the SQL Server container to be READY to accept connections,
        // this is needed on my machine, but it might not be on yours
        // Note that it about READY, not STARTED, so the container might be started, but not ready to accept connections

        Task.Delay(60000).Wait();


        return $"Server={server};Database={database};User ID={user};Password={password};MultipleActiveResultSets=True;TrustServerCertificate=True";

    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services, string connectionString, bool isDevelopment)
        => services
            .AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(connectionString)
                   .EnableSensitiveDataLogging(isDevelopment)
                   .EnableDetailedErrors(isDevelopment));

    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
        => services
            .AddScoped<ICityRepository, CityRepository>()
            .AddScoped<IHotelRepository, HotelRepository>()
            .AddScoped<IRoomRepository, RoomRepository>()
            .AddScoped<IGuestRepository, GuestRepository>()
            .AddScoped<IBookingRepository, BookingRepository>()
            .AddScoped<IGuestRepository, GuestRepository>()
            .AddScoped<IReviewRepository, ReviewRepository>()
            .AddScoped<IDiscountRepository, DiscountRepository>();

    public static IServiceCollection AddImageHandling(
        this IServiceCollection services)
        => services
            .AddScoped<IImageHandler, ImageHandler>();


    // check if there is any pending migration and apply it
    public static IApplicationBuilder Migrate(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        return app;
    }

}
