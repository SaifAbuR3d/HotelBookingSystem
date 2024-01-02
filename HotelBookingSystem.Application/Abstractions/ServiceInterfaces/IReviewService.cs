using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Review.Command;
using HotelBookingSystem.Application.DTOs.Review.OutputModel;
using HotelBookingSystem.Application.DTOs.Review.Query;

namespace HotelBookingSystem.Application.Abstractions.ServiceInterfaces;

public interface IReviewService
{
    Task<ReviewOutputModel> AddReviewAsync(Guid id, CreateOrUpdateReviewCommand request);
    Task<ReviewOutputModel> GetReviewAsync(Guid id, Guid reviewId);
    Task<bool> UpdateReviewAsync(Guid id, Guid reviewId, CreateOrUpdateReviewCommand request);
    Task<bool> DeleteReviewAsync(Guid id, Guid reviewId);
    Task<(IEnumerable<ReviewOutputModel>, PaginationMetadata)> GetHotelReviewsAsync(Guid id, GetHotelReviewsQueryParameters request);
    Task<double> GetHotelAverageRatingAsync(Guid id);
}
