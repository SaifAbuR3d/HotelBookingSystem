using HotelBookingSystem.Application.DTOs.Room;

namespace HotelBookingSystem.Application.ServiceInterfaces;

public interface IRoomService
{
    Task<RoomOutputModel> CreateRoomAsync(CreateRoomCommand command);
    Task<bool> DeleteRoomAsync(Guid id);
    Task<RoomOutputModel?> GetRoomAsync(Guid id);
    Task<IEnumerable<RoomOutputModel>> GetAllRoomsAsync();
    Task<bool> UpdateRoomAsync(Guid id, UpdateRoomCommand request);
}
