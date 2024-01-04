using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Room.Query;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;

public interface IRoomRepository
{
    Task<Room> AddRoomAsync(Room room);
    Task<bool> RoomExistsAsync(Guid id);
    Task<bool> DeleteRoomAsync(Guid id);
    Task<(IEnumerable<Room>, PaginationMetadata)> GetAllRoomsAsync(GetRoomsQueryParameters request);
    Task<IEnumerable<Booking>> GetBookingsForRoomAsync(Guid roomId);
    Task<Room?> GetRoomAsync(Guid id);
    Task<bool> SaveChangesAsync();
    Task<bool> IsAvailableAsync(Guid roomId, DateOnly startDate, DateOnly endDate);
    Task<RoomImage> AddRoomImageAsync(Room room, RoomImage roomImage);
    Task<IEnumerable<Room>> GetRoomsWithHighestDiscounts(int rooms);
}
