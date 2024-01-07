using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Abstractions;

/// <summary>
/// this interface is used to make a user a guest
/// it is created to avoid circular dependency (when registering the user)
/// </summary>
public interface IUser
{
    void BecomeGuest(Guest guest);
}
