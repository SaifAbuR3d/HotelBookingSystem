using AutoMapper;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Mapping;

public class FeaturedDealProfile : Profile
{

    public FeaturedDealProfile()
    {
        var defaultHotelImage = new HotelImage
        {
            ImageUrl = "C:\\Users\\user\\source\\repos\\HotelBookingSystem\\HotelBookingSystem.Api\\wwwroot\\images\\common\\defaultHotel.jpg",
            AlternativeText = "Default hotel image"
        };

        CreateMap<Room, FeaturedDealOutputModel>()
            .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.Hotel.Id))
            .ForMember(dest => dest.HotelImage, opt => opt.MapFrom(src =>
                                src.Hotel.Images.FirstOrDefault(i => i.ImageUrl.Contains("thumbnail"))
                                ?? src.Hotel.Images.FirstOrDefault()
                                ?? defaultHotelImage
                                ))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name))
            .ForMember(dest => dest.StarRate, opt => opt.MapFrom(src => src.Hotel.StarRate))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Hotel.Latitude))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Hotel.Longitude))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Hotel.Street))

            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Hotel.City.Name))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Hotel.City.Country))

            .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OriginalPrice, opt => opt.MapFrom(src => src.Price))

            .ForMember(dest => dest.DiscountedPrice, opt => opt.MapFrom(src => src.Discounts.First().DiscountedPrice))
            .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.Discounts.First().Percentage));

    }
}
