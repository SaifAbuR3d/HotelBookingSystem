using HotelBookingSystem.Domain.Abstractions.Repositories;
using HotelBookingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Persistence.Repositories;

public class HotelRepository(ApplicationDbContext context) : IHotelRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> HotelExistsAsync(Guid id)
    {
        return await _context.Hotels.AnyAsync(h => h.Id == id);
    }
    public async Task<Hotel> AddHotelAsync(Hotel hotel)
    {
        await _context.Hotels.AddAsync(hotel);
        return hotel;
    }
    public async Task<bool> DeleteHotelAsync(Guid id)
    {
        var hotel = await _context.Hotels.FindAsync(id);
        if (hotel is null)
        {
            return false;
        }

        _context.Hotels.Remove(hotel);
        return true;
    }
    public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
    {
        return await _context.Hotels.Include(h => h.City).Include(h => h.Rooms).ToListAsync();
    }
    public async Task<Hotel?> GetHotelAsync(Guid id)
    {
        return await _context.Hotels.FindAsync(id);
    }
    public async Task<Hotel?> GetHotelByNameAsync(string Name)
    {
        return await _context.Hotels.FirstOrDefaultAsync(h => h.Name == Name);
    }
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 1;
    }
}
