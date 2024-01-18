using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Persistence.Repositories;

public class GuestRepository : IGuestRepository
{
    private readonly ApplicationDbContext _context;
    public GuestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guest> AddGuestAsync(Guest guest)
    {
        var entry = await _context.Guests.AddAsync(guest);

        return entry.Entity;
    }

    public async Task<IEnumerable<Booking>> GetBookingsForGuestAsync(Guid guestId)
    {
        return await _context.Bookings.Where(b => b.GuestId == guestId).ToListAsync();
    }

    public async Task<Guest?> GetGuestAsync(Guid guestId)
    {
        return await _context.Guests.FindAsync(guestId);

    }

    public async Task<Guest?> GetGuestByUserIdAsync(string userId)
    {
        return await _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Guest).ThenInclude(g => g.Bookings)
                .Select(u => u.Guest)
                .FirstOrDefaultAsync();
    }

    public async Task<Guid?> GetGuestIdByUserIdAsync(string userId)
    {
        return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Guest != null ? u.Guest.Id : Guid.NewGuid())
                .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves a collection of most recent bookings for a guest, each representing the most recent booking in different hotels.
    /// </summary>
    /// <param name="guestId">The unique identifier of the guest for whom recent bookings are to be retrieved.</param>
    /// <param name="count">The maximum number of recent bookings to retrieve. Default is 5.</param>
    /// <returns>An asynchronous task representing the operation, returning a collection of most recent bookings.</returns>
    /// <remarks>
    /// <para>
    /// This method queries the database for bookings associated with the specified guest, including related room, hotel, and city details.
    /// It then groups the bookings by the unique identifier of the associated hotel (HotelId) and selects the most recent booking from each group.
    /// The result is a collection of most recent bookings, each representing the most recent booking in different hotels, limited to the specified count.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var guestId = Guid.Parse("your_guest_id_here");
    /// var recentBookings = await repository.GetRecentBookingsInDifferentHotels(guestId, 5);
    /// // Process the recentBookings collection as needed.
    /// </code>
    /// </example>
    /// <seealso cref="Booking"/>
    /// </summary>
    /// <returns>An asynchronous task representing the operation, returning a collection of most recent bookings.</returns>

    public async Task<IEnumerable<Booking>> GetRecentBookingsInDifferentHotelsAsync(Guid guestId, int count = 5)
    {
        var guestSortedBookings = await _context.Bookings
            .Where(b => b.GuestId == guestId)
            .Include(r => r.Hotel).ThenInclude(h => h.City)
            .Include(b => b.Hotel.Images)
            .OrderByDescending(b => b.CheckInDate)
            .ToListAsync();

        // Get the most recent booking in each hotel.
        var guestRecentBookingsInDifferentHotels = guestSortedBookings
            .GroupBy(b => b.HotelId)
            .Select(group => group.First())
            .Take(count);


        return guestRecentBookingsInDifferentHotels;
    }

    public async Task<bool> GuestExistsAsync(Guid guestId)
    {
        return await _context.Guests.AnyAsync(g => g.Id == guestId);
    }

    public async Task<bool> HasGuestBookedHotelAsync(Hotel hotel, Guest guest)
    {
        return await _context.Bookings
            .AnyAsync(b => b.GuestId == guest.Id && b.HotelId == hotel.Id);
    }

    public Task<bool> HasGuestReviewedHotelAsync(Hotel hotel, Guest guest)
    {
        return _context.Reviews
            .AnyAsync(r => r.GuestId == guest.Id && r.HotelId == hotel.Id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 1;
    }
}