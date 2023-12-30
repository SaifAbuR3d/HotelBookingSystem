using HotelBookingSystem.Domain.Abstractions.Repositories;
using HotelBookingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Persistence.Repositories;

public class HotelRepository(ApplicationDbContext context) : IHotelRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> HotelExistsAsync(Guid id)
    {
        return await _context.Hotels.AnyAsync(h => h.Id == id);
    }
    public async Task<Hotel> AddHotelAsync(Hotel hotel)
    {
        await _context.Hotels.AddAsync(hotel);
        return hotel;
    }

    public async Task<HotelImage> AddHotelImageAsync(Hotel hotel, HotelImage hotelImage)
    {
        await _context.HotelImages.AddAsync(hotelImage);
        hotel.Images.Add(hotelImage);
        return hotelImage;
    }
    public async Task<Review> AddReviewAsync(Hotel hotel, Review review)
    {
        await _context.Reviews.AddAsync(review);
        hotel.Reviews.Add(review);
        return review;
    }

    public async Task<bool> DeleteHotelAsync(Guid id)
    {
        var hotel = await _context.Hotels.FindAsync(id);
        if (hotel is null)
        {
            return false;
        }

        _context.Hotels.Remove(hotel);
        return true;
    }

    public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
    {
        return await _context.Hotels.Include(h => h.City).Include(h => h.Rooms).ToListAsync();
    }

    public async Task<Hotel?> GetHotelAsync(Guid id)
    {
        return await _context.Hotels.Include(h => h.Images).FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<Hotel?> GetHotelByNameAsync(string Name)
    {
        return await _context.Hotels.FirstOrDefaultAsync(h => h.Name == Name);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<Review?> GetReviewAsync(Hotel hotel, Guid reviewId)
    {
        return await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId && r.HotelId == hotel.Id);
    }

    public async Task<bool> DeleteReviewAsync(Guid id, Guid reviewId)
    {
        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId && r.HotelId == id);
        if (review is null)
        {
            return false;
        }

        _context.Reviews.Remove(review);
        return true;
    }

    public async Task<IEnumerable<Review>> GetHotelReviewsAsync(Hotel hotel)
    {
        return await _context.Reviews
            .Include(r => r.Hotel).Include(r => r.Guest)
            .Where(r => r.HotelId == hotel.Id)
            .ToListAsync();
    }

    public async Task<double> GetHotelAverageRatingAsync(Hotel hotel)
    {
        return await _context.Reviews
            .Where(r => r.HotelId == hotel.Id)
            .AverageAsync(r => r.Rating);
    }
}
