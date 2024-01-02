using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using HotelBookingSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;

namespace HotelBookingSystem.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
        => services
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddScoped<ICityService, CityService>()
            .AddScoped<IHotelService, HotelService>()
            .AddScoped<IRoomService, RoomService>()
            .AddScoped<IBookingService, BookingService>()
            .AddScoped<IGuestService, GuestService>();
}
