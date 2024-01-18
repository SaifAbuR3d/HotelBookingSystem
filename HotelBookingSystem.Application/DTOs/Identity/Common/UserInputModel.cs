namespace HotelBookingSystem.Application.DTOs.Identity.Common;

/// <summary>
/// DTO for user input (login/register)
/// </summary>
public class UserInputModel
{
    public UserInputModel(string email, string password)
    {
        Email = email;
        Password = password;
    }

    /// <summary>
    /// Email address of the user.
    /// </summary>
    public string Email { get; } = default!;

    /// <summary>
    /// Password of the user. 
    /// (the password is Minimum 6 characters, at least one uppercase letter, one lowercase letter, one number and one special character.)
    /// </summary>
    public string Password { get; } = default!;

}
