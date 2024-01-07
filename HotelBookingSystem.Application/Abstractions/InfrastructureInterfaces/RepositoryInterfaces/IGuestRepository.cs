using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;

public interface IGuestRepository
{
    Task<Guest?> GetGuestAsync(Guid guestId);
    Task<IEnumerable<Booking>> GetBookingsForGuestAsync(Guid guestId);
    Task<IEnumerable<Booking>> GetRecentBookingsInDifferentHotelsAsync(Guid guestId, int count = 5);
    Task<bool> HasGuestBookedHotelAsync(Hotel hotel, Guest guest);
    Task<bool> HasGuestReviewedHotelAsync(Hotel hotel, Guest guest);
    Task<Guest?> GetGuestByUserIdAsync(string userId);
    Task<Guest> AddGuestAsync(Guest guest);
    Task<bool> SaveChangesAsync();
    Task<Guid?> GetGuestIdByUserIdAsync(string userId);
    Task<bool> GuestExistsAsync(Guid guestId);
}