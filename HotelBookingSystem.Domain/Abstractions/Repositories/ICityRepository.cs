using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Domain.Abstractions.Repositories;

public interface ICityRepository
{
    City CreateCity(City city); // 
    Task<bool> CityExistsAsync(Guid id);
    Task<IEnumerable<City>> GetAllCitiesAsync();
    Task<City?> GetCityAsync(Guid id);
    Task<bool> SaveChangesAsync();
    Task<bool> DeleteCity(Guid id);
}
