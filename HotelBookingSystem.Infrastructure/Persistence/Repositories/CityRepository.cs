using HotelBookingSystem.Domain.Abstractions.Repositories;
using HotelBookingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Persistence.Repositories;

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

    public async Task<CityImage> AddCityImageAsync(City city, CityImage cityImage)
    {
        await _context.AddAsync(cityImage);
        city.Images.Add(cityImage);
        return cityImage;
    }

    public async Task<bool> DeleteCityAsync(Guid id)
    {
        var city = await _context.Cities.FindAsync(id);
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

    public async Task<IEnumerable<City>> MostVisitedCitiesAsync(int count = 5)
    {
        var cityIds = await _context.Bookings
            .GroupBy(b => b.Room.Hotel.CityId)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => g.Key)
            .ToListAsync();

        var cities = await _context.Cities
            .Where(c => cityIds.Contains(c.Id))
            .Include(c => c.Images)
            .ToListAsync();

        return cities;

    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 1;
    }
}
