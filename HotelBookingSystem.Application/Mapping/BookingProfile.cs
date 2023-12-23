using AutoMapper;
using HotelBookingSystem.Application.DTOs.Booking;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Mapping;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<CreateBookingCommand, Booking>();
        CreateMap<Booking, BookingConfirmationOutputModel>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Room.Hotel.Name))
            .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.Room.RoomType))
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber))
            .ForMember(dest => dest.HotelLocation, opt => opt.MapFrom(src => src.Room.Hotel.Location));

    }

}
