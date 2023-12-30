using AutoMapper;
using HotelBookingSystem.Application.DTOs.Review;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Mapping;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<CreateOrUpdateReviewCommand, Review>();
        CreateMap<Review, ReviewOutputModel>();
    }
}
