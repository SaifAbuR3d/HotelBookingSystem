using AutoMapper;
using HotelBookingSystem.Application.DTOs.City;
using HotelBookingSystem.Application.ServiceInterfaces;
using HotelBookingSystem.Domain.Abstractions.Repositories;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Services;

public class CityService(ICityRepository cityRepository, IMapper mapper) : ICityService // see save changes, consider UoW
{
    private readonly ICityRepository _cityRepository = cityRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CityOutputModel>> GetAllCitiesAsync()
    {
        var cities = await _cityRepository.GetAllCitiesAsync();
        var mapped = _mapper.Map<IEnumerable<CityOutputModel>>(cities);

        return mapped;
    }

    public async Task<CityOutputModel?> GetCityAsync(Guid id)
    {
        var city = await _cityRepository.GetCityAsync(id);
        var mapped = _mapper.Map<CityOutputModel>(city);

        return mapped;
    }
    public async Task<bool> DeleteCityAsync(Guid id)
    {
        var deleted = await _cityRepository.DeleteCityAsync(id);
        if (deleted)
        {
            await _cityRepository.SaveChangesAsync();
        }
        return deleted;
    }

    public async Task<CityOutputModel> CreateCityAsync(CreateCityCommand request)
    {
        var city = _mapper.Map<City>(request);

        city.Id = Guid.NewGuid();
        city.CreationDate = DateTime.UtcNow;
        city.LastModified = DateTime.UtcNow;

        var createdCity = await _cityRepository.AddCityAsync(city);
        await _cityRepository.SaveChangesAsync();

        return _mapper.Map<CityOutputModel>(createdCity);
    }

    public async Task<bool> UpdateCityAsync(Guid id, UpdateCityCommand request)
    {
        var city = await _cityRepository.GetCityAsync(id);
        if (city is null)
        {
            return false;
        }

        _mapper.Map(request, city);

        city.LastModified = DateTime.UtcNow;

        await _cityRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CityExistsAsync(Guid id)
    {
        return await _cityRepository.CityExistsAsync(id);
    }

    
}
