using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Hotel.Query;
using HotelBookingSystem.Domain.Models;
namespace HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;

public interface IHotelRepository
{
    Task<Hotel> AddHotelAsync(Hotel city); // 
    Task<bool> HotelExistsAsync(Guid id);
    Task<bool> DeleteHotelAsync(Guid id);
    Task<IEnumerable<Hotel>> GetAllHotelsAsync();
    Task<Hotel?> GetHotelAsync(Guid id);
    Task<bool> SaveChangesAsync();
    Task<Hotel?> GetHotelByNameAsync(string Name);
    Task<HotelImage> AddHotelImageAsync(Hotel hotel, HotelImage hotelImage);
    Task<Review> AddReviewAsync(Hotel hotel, Review review);
    Task<Review?> GetReviewAsync(Hotel hotel, Guid reviewId);
    Task<bool> DeleteReviewAsync(Guid id, Guid reviewId);
    Task<IEnumerable<Review>> GetHotelReviewsAsync(Hotel hotel);
    Task<double> GetHotelAverageRatingAsync(Hotel hotel);
    Task<(IEnumerable<Hotel>, PaginationMetadata)> SearchAndFilterHotelsAsync(HotelSearchAndFilterParameters request);
}
