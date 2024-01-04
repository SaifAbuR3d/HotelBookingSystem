using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Room.Query;
using HotelBookingSystem.Domain.Models;
using HotelBookingSystem.Infrastructure.Persistence.Helpers;
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

    public async Task<(IEnumerable<Room>, PaginationMetadata)> GetAllRoomsAsync(GetRoomsQueryParameters request)
    {
        var query = _context.Rooms
            .Include(r => r.Hotel)
            .AsQueryable();

        SearchInHotelName(ref query, request.SearchTerm);

        SortingHelper.ApplySorting(ref query, request.SortOrder, SortingHelper.GetRoomsSortingCriterion(request));

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query, request.PageNumber, request.PageSize);

        PaginationHelper.ApplyPagination(ref query, request.PageNumber, request.PageSize);

        var result = await query.ToListAsync();

        return (result, paginationMetadata);
    }

    private static void SearchInHotelName(ref IQueryable<Room> query, string? searchTerm)
    {
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(r => r.Hotel.Name.Contains(searchTerm));
        }
    }

    public async Task<IEnumerable<Room>> GetRoomsWithHighestDiscounts(int rooms)
    {
        var currentDate = DateTime.UtcNow;
        return await _context.Rooms
         .Include(r => r.Discounts)
         .Include(r => r.Hotel.City)
         .Include(r => r.Hotel.Images)
         .Where(r => r.Discounts.Any(d => currentDate >= d.StartDate && currentDate < d.EndDate))

         // if multiple discounts are active for a room, select the most recently added one
         .Select(r => new
         {
             Room = r,
             Percentage = r.Discounts.OrderByDescending(d => d.CreationDate).First().Percentage

         })

         .OrderByDescending(r => r.Percentage)
         .Take(rooms)
         .Select(r => r.Room)
         .ToListAsync();
    }
}
