namespace HotelBookingSystem.Application.Abstractions;

public interface ICurrentUser
{
    string Id { get; }
    string Role { get; }
    bool IsGuest { get; }
    bool IsAdmin { get; }
    string Email { get; }
}
