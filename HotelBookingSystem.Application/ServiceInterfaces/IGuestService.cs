using HotelBookingSystem.Application.DTOs.Hotel;

namespace HotelBookingSystem.Application.ServiceInterfaces;

public interface IGuestService
{
    Task<IEnumerable<RecentlyVisitedHotelOutputModel>> GetRecentlyVisitedHotelsAsync(Guid guestId, int count = 5);
}