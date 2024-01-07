namespace HotelBookingSystem.Application.DTOs.Identity.OutputModel;

public class LoginOutputModel
{
    public LoginOutputModel(Guid guestId, string token)
    {
        this.GuestId = guestId;
        this.Token = token;
    }

    public Guid GuestId { get; }

    public string Token { get; }
}
