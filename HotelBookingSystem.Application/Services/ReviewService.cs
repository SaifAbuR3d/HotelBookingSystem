using AutoMapper;
using HotelBookingSystem.Application.Abstractions;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Review.Command;
using HotelBookingSystem.Application.DTOs.Review.OutputModel;
using HotelBookingSystem.Application.DTOs.Review.Query;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Domain.Models;
using Microsoft.Extensions.Logging;
namespace HotelBookingSystem.Application.Services;
public class ReviewService(IHotelRepository hotelRepository,
                           IGuestRepository guestRepository,
                           IReviewRepository reviewRepository,
                           IMapper mapper,
                           ICurrentUser currentUser,
                           ILogger<ReviewService> logger) : IReviewService
{
    private readonly IHotelRepository _hotelRepository = hotelRepository;
    private readonly IGuestRepository _guestRepository = guestRepository;
    private readonly IReviewRepository _reviewRepository = reviewRepository;

    private readonly IMapper _mapper = mapper;

    private readonly ICurrentUser _currentUser = currentUser;

    private readonly ILogger<ReviewService> _logger = logger;

    public async Task<ReviewOutputModel> AddReviewAsync(Guid hotelId, CreateOrUpdateReviewCommand request)
    {
        _logger.LogInformation("AddReviewAsync started for hotel with ID: {HotelId}", hotelId);

        _logger.LogDebug("Getting the hotel from the repository");
        var hotel = await _hotelRepository.GetHotelAsync(hotelId) ?? throw new NotFoundException(nameof(Hotel), hotelId);

        var (guest, userId) = await GetGuestFromCurrentUser();

        _logger.LogDebug("Validating user access to review the hotel"); 
        await CanGuestAddReview(guest, hotel);

        _logger.LogDebug("Mapping the request model to a Review entity");
        var review = _mapper.Map<Review>(request);
        review.Id = Guid.NewGuid();
        review.CreationDate = DateTime.UtcNow;
        review.LastModified = DateTime.UtcNow;
        review.HotelId = hotel.Id;
        review.GuestId = guest.Id;

        _logger.LogDebug("Add the review to the repository");
        await _reviewRepository.AddReviewAsync(hotel, review);
        await _reviewRepository.SaveChangesAsync();

        _logger.LogDebug("Mapping the Review entity to a ReviewOutputModel");
        var mapped = _mapper.Map<ReviewOutputModel>(review);

        _logger.LogInformation("AddReviewAsync completed successfully for hotel with ID: {HotelId}, from guest with ID: {guestId}", hotelId, guest.Id);
        return mapped;
    }

    private async Task<bool> CanGuestAddReview(Guest guest, Hotel hotel)
    {
        _logger.LogDebug("Check if guest has booked the hotel");
        if (!await _guestRepository.HasGuestBookedHotelAsync(hotel, guest))
        {
            throw new BadRequestException("Guest has not booked this hotel");
        }

        _logger.LogDebug("Check if guest has already reviewed the hotel");
        if (await _guestRepository.HasGuestReviewedHotelAsync(hotel, guest))
        {
            throw new BadRequestException("Guest has already reviewed this hotel");
        }

        return true;
    }

    private async Task<(Guest, string)> GetGuestFromCurrentUser()
    {
        _logger.LogDebug("Getting the user id from CurrentUser");
        var userId = _currentUser.Id;

        _logger.LogDebug("Getting the guest from the repository");
        var guest = await _guestRepository.GetGuestByUserIdAsync(userId) 
            ?? throw new NotFoundException(nameof(Guest),userId );

        return (guest, userId);
    }

    public async Task<bool> UpdateReviewAsync(Guid hotelId, Guid reviewId, CreateOrUpdateReviewCommand request)
    {
        _logger.LogInformation("UpdateReviewAsync started for hotel with ID: {HotelId}, review with ID: {ReviewId}",
            hotelId, reviewId);

        _logger.LogDebug("Getting the hotel from the repository");
        var hotel = await _hotelRepository.GetHotelAsync(hotelId) ?? throw new NotFoundException(nameof(Hotel), hotelId);

        _logger.LogDebug("Getting the review from the repository");
        var review = await _reviewRepository.GetReviewAsync(hotel, reviewId) ?? throw new NotFoundException(nameof(Review), reviewId);

        _logger.LogDebug("Getting guest representing the current user");
        var (guest, userId) = await GetGuestFromCurrentUser();

        _logger.LogDebug("Validating user access to edit the review");
        if (review.GuestId != guest.Id)
        {
            throw new UnauthorizedException(userId, review.GuestId);
        }

        _logger.LogDebug("Mapping the request model to the Review entity");
        _mapper.Map(request, review);
        review.LastModified = DateTime.UtcNow;

        _logger.LogDebug("Saving to database"); 
        await _reviewRepository.SaveChangesAsync();

        _logger.LogInformation("UpdateReviewAsync completed successfully for hotel with ID: {HotelId}, review with ID: {ReviewId}",
            hotelId, reviewId);
        return true;
    }

    public async Task<bool> DeleteReviewAsync(Guid hotelId, Guid reviewId)
    {
        _logger.LogInformation("DeleteReviewAsync started for hotel with ID: {HotelId}, review with ID: {ReviewId}",
            hotelId, reviewId);

        _logger.LogDebug("Getting the hotel from the repository");
        var hotel = await _hotelRepository.GetHotelAsync(hotelId) ?? throw new NotFoundException(nameof(Hotel), hotelId);

        _logger.LogDebug("Getting the review from the repository");
        var review = await _reviewRepository.GetReviewAsync(hotel, reviewId) ?? throw new NotFoundException(nameof(Review), reviewId);

        _logger.LogDebug("Getting guest representing the current user");
        var (guest, userId) = await GetGuestFromCurrentUser();

        _logger.LogDebug("Validating user access to delete the review");
        if (review.GuestId != guest.Id)
        {
            throw new UnauthorizedException(userId, review.GuestId);
        }

        _logger.LogDebug("deleting from the repository"); 
        bool deleted = await _reviewRepository.DeleteReviewAsync(hotelId, reviewId);
        if (deleted)
        {
            _logger.LogDebug("Saving changes to database"); 
            await _reviewRepository.SaveChangesAsync();
        }

        _logger.LogInformation("DeleteReviewAsync completed successfully for hotel with ID: {HotelId}, from guest with ID: {guestId}", hotelId, guest.Id);
        return deleted;
    }

    public async Task<ReviewOutputModel> GetReviewAsync(Guid hotelId, Guid reviewId)
    {
        var hotel = await _hotelRepository.GetHotelAsync(hotelId) ?? throw new NotFoundException(nameof(Hotel), hotelId);

        var review = await _reviewRepository.GetReviewAsync(hotel, reviewId) ?? throw new NotFoundException(nameof(Review), reviewId);
        return _mapper.Map<ReviewOutputModel>(review);

    }

    public async Task<(IEnumerable<ReviewOutputModel>, PaginationMetadata)> GetHotelReviewsAsync(Guid hotelId, GetHotelReviewsQueryParameters request)
    {
        var hotel = await _hotelRepository.GetHotelAsync(hotelId) ?? throw new NotFoundException(nameof(Hotel), hotelId);

        var (reviews, paginationMetadata) = await _reviewRepository.GetHotelReviewsAsync(hotel, request);

        var mapped = _mapper.Map<IEnumerable<ReviewOutputModel>>(reviews);

        return (mapped, paginationMetadata);
    }

    public async Task<double> GetHotelAverageRatingAsync(Guid id)
    {
        _logger.LogInformation("GetHotelAverageRatingAsync started for hotel with ID: {HotelId}", id);

        _logger.LogDebug("Getting the hotel from the repository");
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        _logger.LogDebug("Getting the average rating from the repository");
        var rating = await _reviewRepository.GetHotelAverageRatingAsync(hotel);

        _logger.LogDebug("Rounding the rating to 1 decimal place");
        double roundedRating = double.Round(rating, 1);

        _logger.LogInformation("GetHotelAverageRatingAsync for hotel with ID: {HotelId} completed successfully", id);
        return roundedRating;
    }
}
