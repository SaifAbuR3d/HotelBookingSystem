using HotelBookingSystem.Application.DTOs.Identity.Common;

namespace HotelBookingSystem.Application.DTOs.Identity.Command;

public class LoginUserModel : UserInputModel
{
    public LoginUserModel(string email, string password) : base(email, password)
    {

    }
}
