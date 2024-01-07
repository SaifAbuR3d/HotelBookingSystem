using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.IdentityInterfaces;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Infrastructure.Identity;
using HotelBookingSystem.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBookingSystem.Infrastructure.Persistence;

public static class PersistenceConfiguration
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        => services
            .AddDatabase(configuration.GetConnectionString("SqlServer"), isDevelopment)
            .AddRepositories()
            .AddImageHandling();
            

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

}
