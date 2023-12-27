using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Domain.Abstractions.Repositories;

public interface ICityRepository
{
    Task<City> AddCityAsync(City city); // 
    Task<bool> CityExistsAsync(Guid id);
    Task<IEnumerable<City>> GetAllCitiesAsync();
    Task<City?> GetCityAsync(Guid id);
    Task<bool> SaveChangesAsync();
    Task<bool> DeleteCityAsync(Guid id);
    Task<City?> GetCityByNameAsync(string name);
    Task<IEnumerable<City>> MostVisitedCitiesAsync(int count = 5);
}
