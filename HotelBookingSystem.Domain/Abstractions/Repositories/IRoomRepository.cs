﻿using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Domain.Abstractions.Repositories;

public interface IRoomRepository
{
    Task<Room> AddRoomAsync(Room room);
    Task<bool> RoomExistsAsync(Guid id);
    Task<bool> DeleteRoomAsync(Guid id);
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    Task<Room?> GetRoomAsync(Guid id);
    Task<bool> SaveChangesAsync();
}