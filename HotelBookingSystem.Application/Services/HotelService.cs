using AutoMapper;
using HotelBookingSystem.Application.DTOs.Hotel;
using HotelBookingSystem.Application.ServiceInterfaces;
using HotelBookingSystem.Domain.Abstractions.Repositories;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Services;

public class HotelService(IHotelRepository hotelRepository,
                          ICityRepository cityRepository,
                          IMapper mapper) : IHotelService 
{
    private readonly IHotelRepository _hotelRepository = hotelRepository;
    private readonly ICityRepository _cityRepository = cityRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<HotelOutputModel>> GetAllHotelsAsync()
    {
        var hotels = await _hotelRepository.GetAllHotelsAsync();
        var mapped = _mapper.Map<IEnumerable<HotelOutputModel>>(hotels);

        return mapped;
    }

    public async Task<HotelOutputModel?> GetHotelAsync(Guid id)
    {
        var hotel = await _hotelRepository.GetHotelAsync(id);
        var mapped = _mapper.Map<HotelOutputModel>(hotel);

        return mapped;
    }

    public async Task<bool> DeleteHotelAsync(Guid id)
    {
        var deleted = await _hotelRepository.DeleteHotelAsync(id);
        if (deleted)
        {
            await _hotelRepository.SaveChangesAsync();
        }
        return deleted;
    }

    public async Task<HotelOutputModel> CreateHotelAsync(CreateHotelCommand request)
    {
        var city = await _cityRepository.GetCityByNameAsync(request.CityName);
        if (city is null)
        {
            return null; ////
        }

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
        var city = await _cityRepository.GetCityByNameAsync(request.CityName);
        if (city is null)
        {
            return false;
        }

        var hotel = await _hotelRepository.GetHotelAsync(id);
        if (hotel is null)
        {
            return false;
        }

        _mapper.Map(request, hotel);

        hotel.City = city;
        hotel.LastModified = DateTime.UtcNow;

        await _hotelRepository.SaveChangesAsync();

        return true;
    }
}
