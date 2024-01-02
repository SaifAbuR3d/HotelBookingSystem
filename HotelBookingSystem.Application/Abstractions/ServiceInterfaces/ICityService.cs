using HotelBookingSystem.Application.DTOs.City;
using HotelBookingSystem.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;

namespace HotelBookingSystem.Application.Abstractions.ServiceInterfaces;

public interface ICityService
{
    Task<CityOutputModel> CreateCityAsync(CreateCityCommand request);
    Task<bool> DeleteCityAsync(Guid id);
    Task<(IEnumerable<CityOutputModel>, PaginationMetadata)> GetAllCitiesAsync(ResourceQueryParameters request);
    Task<CityOutputModel?> GetCityAsync(Guid id);
    Task<IEnumerable<CityAsTrendingDestinationOutputModel>> MostVisitedCitiesAsync(int count = 5);
    Task<bool> UpdateCityAsync(Guid id, UpdateCityCommand request);
    Task<bool> UploadImageAsync(Guid cityId, IFormFile file, string basePath, string? alternativeText, bool? thumbnail = false);
}