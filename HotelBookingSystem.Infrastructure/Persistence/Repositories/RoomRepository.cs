using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Persistence.Repositories;

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

    public async Task<RoomImage> AddRoomImageAsync(Room room, RoomImage roomImage)
    {
        await _context.RoomImages.AddAsync(roomImage);
        room.Images.Add(roomImage);
        return roomImage;
    }

    public async Task<bool> DeleteRoomAsync(Guid id)
    {
        var room = await _context.Rooms.FindAsync(id);
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
        return await _context.Rooms.Include(r => r.Hotel).FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<IEnumerable<Booking>> GetBookingsForRoomAsync(Guid roomId)
    {
        return await _context.Bookings.Where(b => b.RoomId == roomId).ToListAsync();
    }

    public async Task<bool> IsAvailableAsync(Guid roomId, DateOnly startDate, DateOnly endDate)
    {

        //bool isAvailable = await _context.Bookings
        //    .Where(b => b.RoomId == roomId)
        //    .AllAsync(b => startDate > b.CheckOutDate || endDate < b.CheckInDate);
        // return isAvailable;



        bool OverlapsWithSomeBooking = await _context.Bookings
            .Where(b => b.RoomId == roomId)
            .AnyAsync(b => startDate <= b.CheckOutDate && endDate >= b.CheckInDate);

        return !OverlapsWithSomeBooking;
    }
}
