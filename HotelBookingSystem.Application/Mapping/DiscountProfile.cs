using AutoMapper;
using HotelBookingSystem.Application.DTOs.Discount;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Mapping;

public class DiscountProfile : Profile
{
    public DiscountProfile()
    {
        CreateMap<Discount, DiscountOutputModel>(); 
    }
}
