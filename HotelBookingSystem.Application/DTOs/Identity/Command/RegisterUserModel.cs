using HotelBookingSystem.Application.DTOs.Identity.Common;

namespace HotelBookingSystem.Application.DTOs.Identity.Command;

public class RegisterUserModel : UserInputModel
{ 
    public RegisterUserModel(string email, string username, string password) : base(email, password)
    {
        Username = username;
    }

    public string Username { get; set; }
}
