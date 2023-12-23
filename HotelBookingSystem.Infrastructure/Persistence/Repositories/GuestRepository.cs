using HotelBookingSystem.Domain.Abstractions.Repositories;
using HotelBookingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Persistence.Repositories;

public class GuestRepository : IGuestRepository
{
    private readonly ApplicationDbContext _context;
    private Guid fakeId = new Guid("2783b59c-f7f8-4b21-b1df-5149fb57984e");
    public GuestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Booking>> GetBookingsForGuestAsync(Guid guestId)
    {
        // Fake implementation
        return await _context.Bookings.Where(b => b.GuestId == fakeId).ToListAsync();
    }

    public async Task<Guest?> GetGuestAsync(Guid guestId)
    {
        // Fake implementation
        return await _context.Guests.FindAsync(fakeId);
            
    }
}
