using HotelBookingSystem.Application.DTOs.Identity.Common;

namespace HotelBookingSystem.Application.DTOs.Identity.Command;

/// <summary>
/// DTO for registering a user
/// </summary>
public class RegisterUserModel : UserInputModel
{ 
    public RegisterUserModel(string email, string username,
        string password, string firstName, string lastName) : base(email, password)
    {
        Username = username;
        FirstName = firstName;
        LastName = lastName;
    }

    /// <summary>
    /// Username for the user
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// First name of the user
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Last name of the user
    /// </summary>
    public string LastName { get; set; }
}
