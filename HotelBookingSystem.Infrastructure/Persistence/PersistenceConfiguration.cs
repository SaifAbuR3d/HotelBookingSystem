using HotelBookingSystem.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBookingSystem.Infrastructure.Persistence
{
    public static class PersistenceConfiguration
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services, IConfiguration configuration)
            => services
                .AddDatabase(configuration.GetConnectionString("SqlServer"))
                .AddRepositories();

        private static IServiceCollection AddDatabase(
            this IServiceCollection services, string connectionString)
            => services
                .AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionString));

        public static IServiceCollection AddRepositories(
            this IServiceCollection services)
            => services
                .AddScoped<ICityRepository, CityRepository>();
    }
}
