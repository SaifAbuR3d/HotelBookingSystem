using AutoMapper;
using HotelBookingSystem.Application.DTOs.Hotel;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Mapping;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<CreateHotelCommand, Hotel>();

        CreateMap<Hotel, HotelOutputModel>()
            .ForMember(dest => dest.RoomsNumber, opt => opt.MapFrom(src => src.Rooms.Count));

        CreateMap<UpdateHotelCommand, Hotel>();
    }
}
