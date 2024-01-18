using AutoMapper;
using HotelBookingSystem.Application.DTOs.Review.Command;
using HotelBookingSystem.Application.DTOs.Review.OutputModel;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Mapping;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<CreateOrUpdateReviewCommand, Review>();
        CreateMap<Review, ReviewOutputModel>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name));
    }
           
}
