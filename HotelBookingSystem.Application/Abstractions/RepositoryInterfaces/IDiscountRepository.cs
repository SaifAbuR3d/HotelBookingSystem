using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;

public interface IDiscountRepository
{
    Task<Discount> AddDiscountAsync(Discount discount);
    Task<bool> SaveChangesAsync();
}
