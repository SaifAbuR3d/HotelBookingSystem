using AutoMapper;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Hotel.Command;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using HotelBookingSystem.Application.DTOs.Hotel.Query;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Services;

public class HotelService(IHotelRepository hotelRepository,
                          ICityRepository cityRepository,
                          IGuestRepository guestRepository,
                          IMapper mapper,
                          IImageHandler imageHandler, 
                          ILogger<HotelService> logger) : IHotelService
{
    private readonly IHotelRepository _hotelRepository = hotelRepository;
    private readonly ICityRepository _cityRepository = cityRepository;
    private readonly IGuestRepository _guestRepository = guestRepository;

    private readonly IMapper _mapper = mapper;

    private readonly IImageHandler _imageHandler = imageHandler;

    private readonly ILogger<HotelService> _logger = logger;


    public async Task<(IEnumerable<HotelOutputModel>, PaginationMetadata)> GetAllHotelsAsync(GetHotelsQueryParameters request)
    {
        var (hotels, paginationMetadata) = await _hotelRepository.GetAllHotelsAsync(request);
        var mapped = _mapper.Map<IEnumerable<HotelOutputModel>>(hotels);

        return (mapped, paginationMetadata);
    }

    public async Task<HotelOutputModel?> GetHotelAsync(Guid id)
    {
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        var mapped = _mapper.Map<HotelOutputModel>(hotel);

        return mapped;
    }

    public async Task<bool> DeleteHotelAsync(Guid id)
    {
        bool deleted = await _hotelRepository.DeleteHotelAsync(id);
        if (deleted)
        {
            await _hotelRepository.SaveChangesAsync();
        }
        return deleted;
    }

    public async Task<HotelOutputModel> CreateHotelAsync(CreateHotelCommand request)
    {
        var city = await _cityRepository.GetCityByNameAsync(request.CityName) ?? throw new NotFoundException(nameof(City), request.CityName);

        var hotel = _mapper.Map<Hotel>(request);

        hotel.Id = Guid.NewGuid();
        hotel.CreationDate = DateTime.UtcNow;
        hotel.LastModified = DateTime.UtcNow;
        hotel.City = city;

        var createdHotel = await _hotelRepository.AddHotelAsync(hotel);
        await _hotelRepository.SaveChangesAsync();

        return _mapper.Map<HotelOutputModel>(createdHotel);
    }

    public async Task<bool> UpdateHotelAsync(Guid id, UpdateHotelCommand request)
    {
        var city = await _cityRepository.GetCityByNameAsync(request.CityName) ?? throw new NotFoundException(nameof(City), request.CityName);

        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        _mapper.Map(request, hotel);

        hotel.City = city;
        hotel.LastModified = DateTime.UtcNow;

        await _hotelRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UploadImageAsync(Guid hotelId, IFormFile file, string basePath, string? alternativeText, bool? thumbnail = false)
    { 
        if (string.IsNullOrWhiteSpace(basePath))
        {
            throw new BadFileException("an error occurred");
        }

        var hotel = await _hotelRepository.GetHotelAsync(hotelId) ?? throw new NotFoundException(nameof(Hotel), hotelId);
 
        var hotelDirectory = Path.Combine(basePath, "images", "hotels", hotelId.ToString());

        var uploadedImageUrl = await _imageHandler.UploadImage(file, hotelDirectory, thumbnail.GetValueOrDefault(false));

        var image = new HotelImage
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            ImageUrl = uploadedImageUrl,
            AlternativeText = alternativeText,
            HotelId = hotel.Id
        };

        await _hotelRepository.AddHotelImageAsync(hotel, image);
        await _hotelRepository.SaveChangesAsync();

        return true;
    }

    public async Task<(IEnumerable<HotelSearchResultOutputModel>, PaginationMetadata)> SearchAndFilterHotelsAsync(HotelSearchAndFilterParameters request)
    {
        _logger.LogInformation("SearchAndFilterHotelsAsync started with request: {@HotelSearchAndFilterParameters}", request);

        _logger.LogDebug("Searching and filtering hotels from the repository");
        var (hotels, paginationMetadata) = await _hotelRepository.SearchAndFilterHotelsAsync(request);

        _logger.LogDebug("Mapping the Hotel Entities to HotelSearchResultOutputModel");
        var mapped = _mapper.Map<IEnumerable<HotelSearchResultOutputModel>>(hotels);

        _logger.LogInformation("SearchAndFilterHotelsAsync completed successfully with request: {@HotelSearchAndFilterParameters}", request);
        return (mapped, paginationMetadata);
    }
}
