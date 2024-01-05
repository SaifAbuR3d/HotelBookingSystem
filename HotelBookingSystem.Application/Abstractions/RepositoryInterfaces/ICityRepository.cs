using HotelBookingSystem.Application.DTOs.City.Query;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;

public interface ICityRepository
{
    Task<City> AddCityAsync(City city);
    Task<bool> CityExistsAsync(Guid id);
    Task<(IEnumerable<City>, PaginationMetadata)> GetAllCitiesAsync(GetCitiesQueryParameters request);
    Task<City?> GetCityAsync(Guid id);
    Task<bool> SaveChangesAsync();
    Task<bool> DeleteCityAsync(Guid id);
    Task<City?> GetCityByNameAsync(string name);
    Task<IEnumerable<City>> MostVisitedCitiesAsync(int count = 5);
    Task<CityImage> AddCityImageAsync(City city, CityImage cityImage);
}
