namespace HotelBookingSystem.Application.DTOs.Identity.Common;

public class UserInputModel
{
    public UserInputModel(string email, string password)
    {
        Email = email;
        Password = password;
    }
    public string Email { get; } = default!;
    public string Password { get; } = default!;

}
