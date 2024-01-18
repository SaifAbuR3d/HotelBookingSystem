using AutoMapper;
using HotelBookingSystem.Application.DTOs.Discount;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Mapping;

public class DiscountProfile : Profile
{
    public DiscountProfile()
    {
        CreateMap<Discount, DiscountOutputModel>()
            // calculated at service
            .ForMember(dest => dest.OriginalPrice, opt => opt.Ignore())
            .ForMember(dest => dest.DiscountedPrice, opt => opt.Ignore()); 

    }
}
