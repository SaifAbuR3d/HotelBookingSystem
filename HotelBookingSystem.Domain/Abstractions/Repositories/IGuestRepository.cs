using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Domain.Abstractions.Repositories;

public interface IGuestRepository
{
    Task<Guest?> GetGuestAsync(Guid guestId);
    Task<IEnumerable<Booking>> GetBookingsForGuestAsync(Guid guestId);

}
