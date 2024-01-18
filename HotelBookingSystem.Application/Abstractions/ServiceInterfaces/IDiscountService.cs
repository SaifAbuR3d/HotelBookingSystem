using HotelBookingSystem.Application.DTOs.Discount;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;

namespace HotelBookingSystem.Application.Abstractions.ServiceInterfaces;

public interface IDiscountService
{
    Task<DiscountOutputModel> AddDiscountAsync(Guid roomId, CreateDiscountCommand command);
    Task<bool> DeleteDiscountAsync(Guid roomId, Guid discountId);
    Task<DiscountOutputModel?> GetDiscountAsync(Guid roomId, Guid id);
    Task<IEnumerable<FeaturedDealOutputModel>> GetFeaturedDealsAsync(int deals = 5); 
}
