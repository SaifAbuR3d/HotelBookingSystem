using HotelBookingSystem.Application.DTOs.Identity.Command;
using HotelBookingSystem.Application.DTOs.Identity.OutputModel;

namespace HotelBookingSystem.Application.Abstractions.ServiceInterfaces;

public interface IIdentityService
{
    Task<IUser> RegisterUser(RegisterUserModel model, string role);
    Task<LoginOutputModel> Login(LoginUserModel model);
}
