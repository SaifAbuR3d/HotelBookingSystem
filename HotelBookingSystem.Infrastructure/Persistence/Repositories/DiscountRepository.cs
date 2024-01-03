using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Domain.Models;

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
        var entry = _context.Discounts.Add(discount);
        await _context.SaveChangesAsync();

        return entry.Entity;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 1;
    }
}

