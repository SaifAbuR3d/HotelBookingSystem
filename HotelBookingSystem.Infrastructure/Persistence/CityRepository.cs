using HotelBookingSystem.Domain.Abstractions.Repositories;
using HotelBookingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Persistence;

public class CityRepository(ApplicationDbContext context) : ICityRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> CityExistsAsync(Guid id)
    {
        return await _context.Cities.AnyAsync(c => c.Id == id);
    }

    public async Task<City> AddCityAsync(City city)
    {
        await _context.Cities.AddAsync(city);
        return city; 
    }
    public async Task<bool> DeleteCityAsync(Guid id)
    {
        var city = await GetCityAsync(id); 
        if (city is null)
        {
            return false;
        }

        _context.Cities.Remove(city);

        return true; 
    }

    public async Task<IEnumerable<City>> GetAllCitiesAsync()
    {
        return await _context.Cities.Include(c => c.Hotels).ToListAsync();
    }

    public async Task<City?> GetCityAsync(Guid id)
    {
         return await _context.Cities.FindAsync(id);
    }

    public async Task<City?> GetCityByNameAsync(string name)
    {
        return await _context.Cities.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 1); 
    }
}
