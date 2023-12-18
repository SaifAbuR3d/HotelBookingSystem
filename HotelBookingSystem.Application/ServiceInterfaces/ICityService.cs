using HotelBookingSystem.Application.DTOs.City;

namespace HotelBookingSystem.Application.ServiceInterfaces;

public interface ICityService
{
    Task<CityOutputModel> CreateCityAsync(CreateCityCommand request);
    Task<bool> DeleteCityAsync(Guid id);
    Task<IEnumerable<CityOutputModel>> GetAllCitiesAsync();
    Task<CityOutputModel?> GetCityAsync(Guid id);
    Task<bool> UpdateCityAsync(Guid id, UpdateCityCommand request);
}