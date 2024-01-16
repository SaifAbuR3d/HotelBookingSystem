using HotelBookingSystem.Application.DTOs.Identity.Common;

namespace HotelBookingSystem.Application.DTOs.Identity.Command;

/// <summary>
/// DTO for logging in a user
/// </summary>
public class LoginUserModel : UserInputModel
{
    public LoginUserModel(string email, string password) : base(email, password)
    {

    }
}
