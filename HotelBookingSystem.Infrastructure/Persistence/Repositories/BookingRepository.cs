using HotelBookingSystem.Domain.Abstractions.Repositories;
using HotelBookingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Persistence.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;
    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetBookingAsync(Guid bookingId)
    {
        return await _context.Bookings
            .Include(b => b.Guest)
            .Include(b => b.Room).ThenInclude(r => r.Hotel)
            .FirstOrDefaultAsync(b => b.Id == bookingId);
    }

    public async Task<Booking?> GetBookingAsync(Guid guestId, Guid bookingId)
    {
        return await _context.Bookings
            .Include(b => b.Guest)
            .Include(b => b.Room).ThenInclude(r => r.Hotel)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.GuestId == guestId);
    }

    public async Task<Booking> AddBookingAsync(Booking booking)
    {
        var entry = await _context.Bookings.AddAsync(booking);
        return entry.Entity;
    }

    public async Task<bool> DeleteBookingAsync(Guid id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking is null)
        {
            return false; 
        }

        _context.Bookings.Remove(booking);
        return true;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
