using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;

public interface IDiscountRepository
{
    Task<Discount> AddDiscountAsync(Discount discount);
    Task<bool> DeleteDiscountAsync(Guid roomId, Guid discountId);
    Task<Discount?> GetDiscountAsync(Guid roomId, Guid discountId);
    Task<bool> SaveChangesAsync();
}
