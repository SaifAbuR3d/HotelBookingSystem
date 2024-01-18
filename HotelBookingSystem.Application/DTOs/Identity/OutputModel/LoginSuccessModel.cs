namespace HotelBookingSystem.Application.DTOs.Identity.OutputModel;

public class LoginSuccessModel
{
    public LoginSuccessModel(string userId, string token, string role)
    {
        UserId = userId;
        Token = token;
        Role = role;
    }
    public string UserId { get; }
    public string Role { get; }
    public string Token { get; }
}
