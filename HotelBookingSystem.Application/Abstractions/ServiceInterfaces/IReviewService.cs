using HotelBookingSystem.Application.DTOs.Review;

namespace HotelBookingSystem.Application.Abstractions.ServiceInterfaces;

public interface IReviewService
{
    Task<ReviewOutputModel> AddReviewAsync(Guid id, CreateOrUpdateReviewCommand request);
    Task<ReviewOutputModel> GetReviewAsync(Guid id, Guid reviewId);
    Task<bool> UpdateReviewAsync(Guid id, Guid reviewId, CreateOrUpdateReviewCommand request);
    Task<bool> DeleteReviewAsync(Guid id, Guid reviewId);
    Task<IEnumerable<ReviewOutputModel>> GetHotelReviewsAsync(Guid id);
    Task<double> GetHotelAverageRatingAsync(Guid id);
}
