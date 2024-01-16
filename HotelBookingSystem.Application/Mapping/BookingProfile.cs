using AutoMapper;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.DTOs.Booking.OutputModel;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Mapping;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<Booking, BookingOutputModel>()
            .ForMember(dest => dest.ConfirmationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RoomNumbers, opt => opt.MapFrom(src => src.Rooms.Select(r => r.RoomNumber)))
            .ForMember(dest => dest.GuestFullName, opt => opt.MapFrom(src => src.Guest.FullName))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name));


        CreateMap<Booking, Invoice>()
            .ForMember(dest => dest.ConfirmationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.GuestId, opt => opt.MapFrom(src => src.GuestId))
            .ForMember(dest => dest.GuestFullName, opt => opt.MapFrom(src => src.Guest.FullName))
            .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.CheckInDate))
            .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.CheckOutDate))
            .ForMember(dest => dest.NumberOfAdults, opt => opt.MapFrom(src => src.NumberOfAdults))
            .ForMember(dest => dest.NumberOfChildren, opt => opt.MapFrom(src => src.NumberOfChildren))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Price))

            .ForMember(dest => dest.Rooms, opt => opt.Ignore())
            .ForMember(dest => dest.TotalPriceAfterDiscount, opt => opt.Ignore());
            
    }

}
