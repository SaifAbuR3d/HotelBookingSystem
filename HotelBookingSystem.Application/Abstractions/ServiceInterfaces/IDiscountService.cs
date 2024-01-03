using HotelBookingSystem.Application.DTOs.Discount;

namespace HotelBookingSystem.Application.Abstractions.ServiceInterfaces;

public interface IDiscountService
{
    Task<DiscountOutputModel> AddDiscountAsync(CreateDiscountCommand command);
}
