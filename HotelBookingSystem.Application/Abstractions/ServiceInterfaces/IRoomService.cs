using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Room.Command;
using HotelBookingSystem.Application.DTOs.Room.OutputModel;
using HotelBookingSystem.Application.DTOs.Room.Query;
using Microsoft.AspNetCore.Http;

namespace HotelBookingSystem.Application.Abstractions.ServiceInterfaces;

public interface IRoomService
{
    Task<RoomOutputModel> CreateRoomAsync(CreateRoomCommand command);
    Task<bool> DeleteRoomAsync(Guid id);
    Task<RoomOutputModel?> GetRoomAsync(Guid id);
    Task<(IEnumerable<RoomOutputModel>, PaginationMetadata)> GetAllRoomsAsync(GetRoomsQueryParameters parameters);
    Task<bool> UpdateRoomAsync(Guid id, UpdateRoomCommand request);
    Task<bool> UploadImageAsync(Guid roomId, IFormFile file, string basePath, string? alternativeText, bool? thumbnail = false);
}
