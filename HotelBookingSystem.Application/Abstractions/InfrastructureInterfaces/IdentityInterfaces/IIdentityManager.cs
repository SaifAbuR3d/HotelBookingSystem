using HotelBookingSystem.Application.DTOs.Identity.Command;
using HotelBookingSystem.Application.DTOs.Identity.OutputModel;

namespace HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.IdentityInterfaces;

public interface IIdentityManager
{
    Task<LoginSuccessModel> Login(LoginUserModel model);
    Task<IUser> RegisterUser(RegisterUserModel model, string role); // returns created user id
}
