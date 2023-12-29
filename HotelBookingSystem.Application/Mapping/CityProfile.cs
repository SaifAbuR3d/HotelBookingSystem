﻿using AutoMapper;
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

        CreateMap<CityImage, CityImageOutputModel>();

        var defaultCityImage = new CityImage
        {
            ImageUrl = "C:\\Users\\user\\source\\repos\\HotelBookingSystem\\HotelBookingSystem.Api\\wwwroot\\images\\common\\defaultCity.jpg",
            AlternativeText = "Default city image"
        };

        CreateMap<City, CityAsTrendingDestinationOutputModel>()
            .ForMember(dest => dest.CityImage, opt => opt.MapFrom(src =>
                                src.Images.FirstOrDefault(i => i.ImageUrl.Contains("thumbnail"))
                                ?? src.Images.FirstOrDefault()
                                ?? defaultCityImage
                                )
                      );
    }
}
