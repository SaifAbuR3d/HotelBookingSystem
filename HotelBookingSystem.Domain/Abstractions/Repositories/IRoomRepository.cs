using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Domain.Abstractions.Repositories;

public interface IRoomRepository
{
    Task<Room> AddRoomAsync(Room room);
    Task<bool> RoomExistsAsync(Guid id);
    Task<bool> DeleteRoomAsync(Guid id);
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    Task<IEnumerable<Booking>> GetBookingsForRoomAsync(Guid roomId);
    Task<Room?> GetRoomAsync(Guid id);
    Task<bool> SaveChangesAsync();
    Task<bool> IsAvailableAsync(Guid roomId, DateOnly startDate, DateOnly endDate);
}
