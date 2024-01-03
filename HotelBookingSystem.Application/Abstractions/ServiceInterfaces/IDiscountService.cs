using HotelBookingSystem.Application.DTOs.Discount;

namespace HotelBookingSystem.Application.Abstractions.ServiceInterfaces;

public interface IDiscountService
{
    Task<DiscountOutputModel> AddDiscountAsync(Guid roomId, CreateDiscountCommand command);
    Task<bool> DeleteDiscountAsync(Guid roomId, Guid discountId);
    Task<DiscountOutputModel?> GetDiscountAsync(Guid roomId, Guid id);
}
