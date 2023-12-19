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
    }
}
