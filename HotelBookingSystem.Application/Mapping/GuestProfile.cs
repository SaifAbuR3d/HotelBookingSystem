using AutoMapper;
using HotelBookingSystem.Application.DTOs.Guest;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Mapping;

public class GuestProfile : Profile
{
    public GuestProfile()
    {
        CreateMap<Guest, GuestOutputModel>()
            .ForMember(dest => dest.NumberOfBookings, opt => opt.MapFrom(src => src.Bookings.Count)); 
    }
}
