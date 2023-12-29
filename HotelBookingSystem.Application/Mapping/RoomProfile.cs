using AutoMapper;
using HotelBookingSystem.Application.DTOs.Room;
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

        var defaultRoomImage = new RoomImage
        {
            ImageUrl = "C:\\Users\\user\\source\\repos\\HotelBookingSystem\\HotelBookingSystem.Api\\wwwroot\\images\\common\\defaultRoom.jpg",
            AlternativeText = "Default room image"
        };

        CreateMap<Room, RoomWithImageOutputModel>()
            .ForMember(dest => dest.RoomImage, opt => opt.MapFrom(src =>
                                src.Images.FirstOrDefault(i => i.ImageUrl.Contains("thumbnail")) 
                                ?? src.Images.FirstOrDefault() 
                                ?? defaultRoomImage          
                                )
                      );

    }
}
