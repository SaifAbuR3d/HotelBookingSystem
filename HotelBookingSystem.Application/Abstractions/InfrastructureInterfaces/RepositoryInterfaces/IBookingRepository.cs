using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;

public interface IBookingRepository
{
    Task<Booking?> GetBookingAsync(Guid bookingId);
    Task<Booking?> GetBookingAsync(Guid guestId, Guid bookingId);
    Task<Booking> AddBookingAsync(Booking booking);
    Task<bool> DeleteBookingAsync(Guid id);
    Task<bool> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
