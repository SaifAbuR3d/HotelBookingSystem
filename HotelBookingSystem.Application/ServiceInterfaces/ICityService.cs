using HotelBookingSystem.Application.DTOs.City;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.ServiceInterfaces
{
    public interface ICityService
    {
        CityOutputModel CreateCity(CreateCityCommand request);
        Task<bool> DeleteCityAsync(Guid id);
        Task<IEnumerable<CityOutputModel>> GetAllCitiesAsync();
        Task<CityOutputModel?> GetCityAsync(Guid id);
        Task<bool> UpdateCityAsync(Guid id, UpdateCityCommand request);
    }
}