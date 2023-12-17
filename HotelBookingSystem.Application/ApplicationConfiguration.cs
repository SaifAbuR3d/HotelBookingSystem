using HotelBookingSystem.Application.ServiceInterfaces;
using HotelBookingSystem.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HotelBookingSystem.Application
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
            => services
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddScoped<ICityService, CityService>(); 
    }
}
