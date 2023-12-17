using AutoMapper;
using HotelBookingSystem.Application.DTOs.City;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Mapping;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<CreateCityCommand, City>();

        CreateMap<City, CityOutputModel>()
            .ForMember(dest => dest.NumberOfHotels, opt => opt.MapFrom(src => src.Hotels.Count)); 
        
        CreateMap<UpdateCityCommand, City>();
    }
}
