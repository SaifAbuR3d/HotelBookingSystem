using AutoMapper;
using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.City.Command;
using HotelBookingSystem.Application.DTOs.City.OutputModel;
using HotelBookingSystem.Application.DTOs.City.Query;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace HotelBookingSystem.Application.Services;

public class CityService(ICityRepository cityRepository,
                         IMapper mapper,
                         IImageHandler imageHandler) : ICityService
{
    private readonly ICityRepository _cityRepository = cityRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IImageHandler _imageHandler = imageHandler;

    public async Task<(IEnumerable<CityOutputModel>, PaginationMetadata)> GetAllCitiesAsync(GetCitiesQueryParameters request)
    {
        var (cities, paginationMetadata) = await _cityRepository.GetAllCitiesAsync(request);
        var mapped = _mapper.Map<IEnumerable<CityOutputModel>>(cities);

        return (mapped, paginationMetadata);
    }

    public async Task<CityOutputModel?> GetCityAsync(Guid id)
    {
        var city = await _cityRepository.GetCityAsync(id) ?? throw new NotFoundException(nameof(City), id);

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
        var city = await _cityRepository.GetCityAsync(id) ?? throw new NotFoundException(nameof(City), id);

        _mapper.Map(request, city);

        city.LastModified = DateTime.UtcNow;

        await _cityRepository.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<CityAsTrendingDestinationOutputModel>> MostVisitedCitiesAsync(int count = 5)
    {
        var cities = await _cityRepository.MostVisitedCitiesAsync(count);
        return _mapper.Map<IEnumerable<CityAsTrendingDestinationOutputModel>>(cities);
    }

    public async Task<bool> UploadImageAsync(Guid cityId, IFormFile file, string basePath, string? alternativeText, bool? thumbnail = false)
    {
        if (string.IsNullOrWhiteSpace(basePath))
        {
            throw new BadFileException("an error occurred");
        }

        var city = await _cityRepository.GetCityAsync(cityId) ?? throw new NotFoundException(nameof(City), cityId);

        var cityDirectory = Path.Combine(basePath, "images", "cities", cityId.ToString());

        var uploadedImageUrl = await _imageHandler.UploadImage(file, cityDirectory, thumbnail.GetValueOrDefault(false));

        var image = new CityImage
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            ImageUrl = uploadedImageUrl,
            AlternativeText = alternativeText,
            CityId = city.Id
        };

        await _cityRepository.AddCityImageAsync(city, image);
        await _cityRepository.SaveChangesAsync();

        return true;
    }


    public async Task<bool> CityExistsAsync(Guid id)
    {
        return await _cityRepository.CityExistsAsync(id);
    }




    
}
