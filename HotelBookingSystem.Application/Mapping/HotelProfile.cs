using AutoMapper;
using HotelBookingSystem.Application.DTOs.Hotel.Command;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
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

        CreateMap<HotelImage, HotelImageOutputModel>();

        var defaultHotelImage = new HotelImage
        {
            ImageUrl = "C:\\Users\\user\\source\\repos\\HotelBookingSystem\\HotelBookingSystem.Api\\wwwroot\\images\\common\\defaultHotel.jpg",
            AlternativeText = "Default hotel image"
        };

        CreateMap<Booking, RecentlyVisitedHotelOutputModel>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Room.Hotel.Name))
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Room.Hotel.City.Name))
            .ForMember(dest => dest.StarRating, opt => opt.MapFrom(src => src.Room.Hotel.StarRate))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.HotelImage, opt => opt.MapFrom(src =>
                                src.Room.Hotel.Images.FirstOrDefault(i => i.ImageUrl.Contains("thumbnail"))
                                ?? src.Room.Hotel.Images.FirstOrDefault()
                                ?? defaultHotelImage
                                )
                      );

        CreateMap<Hotel, HotelWithRoomsAndImagesOutputModel>()
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.HotelImages, opt => opt.MapFrom(src => src.Images.Take(5)))
            .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms.Take(10)));


        CreateMap<Hotel, HotelSearchResultOutputModel>()
            .ForMember(dest => dest.HotelImage, opt => opt.MapFrom(src =>
                                src.Images.FirstOrDefault(i => i.ImageUrl.Contains("thumbnail"))
                                ?? src.Images.FirstOrDefault()
                                ?? defaultHotelImage
                                ))
            .ForMember(dest => dest.PriceStartingFrom, opt => opt.MapFrom(src =>
                                src.Rooms.Count() > 0 ? src.Rooms.Min(r => r.Price) : 0
                                ));
    }
}
