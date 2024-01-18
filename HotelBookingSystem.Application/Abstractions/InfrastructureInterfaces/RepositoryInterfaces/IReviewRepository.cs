using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Review.Query;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;

public interface IReviewRepository
{
    Task<Review> AddReviewAsync(Hotel hotel, Review review);
    Task<Review?> GetReviewAsync(Hotel hotel, Guid reviewId);
    Task<bool> DeleteReviewAsync(Guid id, Guid reviewId);
    Task<(IEnumerable<Review>, PaginationMetadata)> GetHotelReviewsAsync(Hotel hotel, GetHotelReviewsQueryParameters request);
    Task<double> GetHotelAverageRatingAsync(Hotel hotel);
    Task<bool> SaveChangesAsync();

}
