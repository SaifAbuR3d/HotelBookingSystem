using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;

public interface IReviewRepository
{
    Task<Review> AddReviewAsync(Hotel hotel, Review review);
    Task<Review?> GetReviewAsync(Hotel hotel, Guid reviewId);
    Task<bool> DeleteReviewAsync(Guid id, Guid reviewId);
    Task<IEnumerable<Review>> GetHotelReviewsAsync(Hotel hotel);
    Task<double> GetHotelAverageRatingAsync(Hotel hotel);
    Task<bool> SaveChangesAsync();

}
