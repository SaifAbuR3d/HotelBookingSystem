using AutoMapper;
using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
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
                           ILogger<ReviewService> logger) : IReviewService
{
    private readonly IHotelRepository _hotelRepository = hotelRepository;
    private readonly IGuestRepository _guestRepository = guestRepository;
    private readonly IReviewRepository _reviewRepository = reviewRepository;

    private readonly IMapper _mapper = mapper;

    private readonly ILogger<ReviewService> _logger = logger;

    public async Task<ReviewOutputModel> AddReviewAsync(Guid id, CreateOrUpdateReviewCommand request)
    {
        _logger.LogInformation("AddReviewAsync started for hotel with ID: {HotelId}", id);
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        // TODO get the guestId from identity service or whatever 
        _logger.LogInformation("Getting the guest from the identity service");
        var guestId = new Guid("2783b59c-f7f8-4b21-b1df-5149fb57984e"); // just for testing,
                                                                        // other guests: 
                                                                        // 2983b59c-f7f8-4b21-b1df-5149fb57984e  2883b59c-f7f8-4b21-b1df-5149fb57984e

        _logger.LogDebug("Getting the guest from the repository");
        var guest = await _guestRepository.GetGuestAsync(guestId) ?? throw new NotFoundException(nameof(Guest), guestId);

        _logger.LogDebug("Mapping the request model to a Review entity");
        var review = _mapper.Map<Review>(request);

        review.Id = Guid.NewGuid();
        review.CreationDate = DateTime.UtcNow;
        review.LastModified = DateTime.UtcNow;

        review.HotelId = hotel.Id;
        review.GuestId = guestId;

        _logger.LogInformation("Validate the request model");

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

        _logger.LogDebug("Add the review to the repository"); 
        await _reviewRepository.AddReviewAsync(hotel, review);
        await _reviewRepository.SaveChangesAsync();

        _logger.LogDebug("Mapping the Review entity to a ReviewOutputModel"); 
        var mapped = _mapper.Map<ReviewOutputModel>(review);

        _logger.LogInformation("AddReviewAsync completed successfully for hotel with ID: {HotelId}, from guest with ID: {guestId}", id, guestId);
        return mapped; 
    }

    public async Task<ReviewOutputModel> GetReviewAsync(Guid id, Guid reviewId)
    {
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        var review = await _reviewRepository.GetReviewAsync(hotel, reviewId) ?? throw new NotFoundException(nameof(Review), reviewId);
        return _mapper.Map<ReviewOutputModel>(review);

    }

    public async Task<(IEnumerable<ReviewOutputModel>, PaginationMetadata)> GetHotelReviewsAsync(Guid id, GetHotelReviewsQueryParameters request)
    {
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        var (reviews, paginationMetadata) = await _reviewRepository.GetHotelReviewsAsync(hotel, request);

        var mapped = _mapper.Map<IEnumerable<ReviewOutputModel>>(reviews);

        return (mapped, paginationMetadata); 
    }

    public async Task<bool> UpdateReviewAsync(Guid id, Guid reviewId, CreateOrUpdateReviewCommand request)
    {
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        var review = await _reviewRepository.GetReviewAsync(hotel, reviewId) ?? throw new NotFoundException(nameof(Review), reviewId);

        _mapper.Map(request, review);
        review.LastModified = DateTime.UtcNow;

        await _reviewRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteReviewAsync(Guid id, Guid reviewId)
    {
        bool deleted = await _reviewRepository.DeleteReviewAsync(id, reviewId);
        if (deleted)
        {
            await _reviewRepository.SaveChangesAsync();
        }
        return deleted;
    }

    public async Task<double> GetHotelAverageRatingAsync(Guid id)
    {
        _logger.LogInformation("GetHotelAverageRatingAsync started for hotel with ID: {HotelId}", id);

        _logger.LogDebug("Getting the hotel from the repository");
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        _logger.LogDebug("Getting the average rating from the repository"); 
        var rating = await _reviewRepository.GetHotelAverageRatingAsync(hotel);

        _logger.LogInformation("GetHotelAverageRatingAsync for hotel with ID: {HotelId} completed successfully", id);
        return double.Round(rating, 1);

    }
}
