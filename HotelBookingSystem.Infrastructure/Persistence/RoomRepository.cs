﻿using HotelBookingSystem.Domain.Abstractions.Repositories;
using HotelBookingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Persistence;

public class RoomRepository(ApplicationDbContext context) : IRoomRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> RoomExistsAsync(Guid id)
    {
        return await _context.Rooms.AnyAsync(h => h.Id == id);
    }
    public async Task<Room> AddRoomAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
        return room;
    }
    public async Task<bool> DeleteRoomAsync(Guid id)
    {
        var room = await GetRoomAsync(id);
        if (room is null)
        {
            return false;
        }

        _context.Rooms.Remove(room);
        return true;
    }
    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        return await _context.Rooms.Include(r => r.Hotel).ToListAsync(); 
    }
    public async Task<Room?> GetRoomAsync(Guid id)
    {
        return await _context.Rooms.FindAsync(id);
    }
    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 1);
    }
}