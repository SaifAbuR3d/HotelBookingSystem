using AutoMapper;
using HotelBookingSystem.Application.DTOs.Room.Command;
using HotelBookingSystem.Application.DTOs.Room.OutputModel;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Mapping;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<CreateRoomCommand, Room>();

        CreateMap<Room, RoomOutputModel>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name));

        CreateMap<UpdateRoomCommand, Room>();

        CreateMap<RoomImage, RoomImageOutputModel>();

        CreateMap<Room, RoomWithImageOutputModel>()
            .ForMember(dest => dest.RoomImage, opt => opt.MapFrom(src =>
                                src.Images.FirstOrDefault(i => i.ImageUrl.Contains("thumbnail")) 
                                ?? src.Images.FirstOrDefault() 
                                )
                      );

        CreateMap<Room, RoomWithinInvoice>()
            .ForMember(dest => dest.PricePerNight, opt => opt.Ignore())
            .ForMember(dest => dest.PricePerNightAfterDiscount, opt => opt.Ignore())
            .ForMember(dest => dest.TotalRoomPrice, opt => opt.Ignore())
            .ForMember(dest => dest.TotalRoomPriceAfterDiscount, opt => opt.Ignore())
            .ForMember(dest => dest.NumberOfNights, opt => opt.Ignore()); 

    }
}
