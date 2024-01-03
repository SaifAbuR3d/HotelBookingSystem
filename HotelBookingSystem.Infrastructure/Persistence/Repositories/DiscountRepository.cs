using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Persistence.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly ApplicationDbContext _context;

    public DiscountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Discount> AddDiscountAsync(Discount discount)
    {
        var entry = await _context.Discounts.AddAsync(discount);

        return entry.Entity;
    }

    public async Task<bool> DeleteDiscountAsync(Guid roomId, Guid discountId)
    {
        var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.RoomId ==roomId && d.Id == discountId);

        if (discount is null)
        {
            return false;
        }

        _context.Discounts.Remove(discount);

        return true;
    }

    public async Task<Discount?> GetDiscountAsync(Guid roomId, Guid discountId)
    {
        return await _context.Discounts
            .Include(d => d.Room)
            .FirstOrDefaultAsync(d => d.RoomId == roomId && d.Id == discountId);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 1;
    }
}

