using HotelBookingSystem.Application.DTOs.Room;
using Microsoft.AspNetCore.Http;

namespace HotelBookingSystem.Application.ServiceInterfaces;

public interface IRoomService
{
    Task<RoomOutputModel> CreateRoomAsync(CreateRoomCommand command);
    Task<bool> DeleteRoomAsync(Guid id);
    Task<RoomOutputModel?> GetRoomAsync(Guid id);
    Task<IEnumerable<RoomOutputModel>> GetAllRoomsAsync();
    Task<bool> UpdateRoomAsync(Guid id, UpdateRoomCommand request);
    Task<bool> UploadImageAsync(Guid roomId, IFormFile file, string basePath, string? alternativeText, bool? thumbnail = false);
}
