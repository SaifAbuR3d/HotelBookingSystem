using AutoMapper;
using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Review;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Domain.Models;
namespace HotelBookingSystem.Application.Services;
public class ReviewService(IHotelRepository hotelRepository,
                           IGuestRepository guestRepository,
                           IReviewRepository reviewRepository, 
                           IMapper mapper) : IReviewService
{
    private readonly IHotelRepository _hotelRepository = hotelRepository;
    private readonly IGuestRepository _guestRepository = guestRepository;
    private readonly IReviewRepository _reviewRepository = reviewRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ReviewOutputModel> AddReviewAsync(Guid id, CreateOrUpdateReviewCommand request)
    {
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        // get the guestId from identity service or whatever 
        var guestId = new Guid("2783b59c-f7f8-4b21-b1df-5149fb57984e"); // just for testing,
                                                                        // other guests: 
                                                                        // 2983b59c-f7f8-4b21-b1df-5149fb57984e  2883b59c-f7f8-4b21-b1df-5149fb57984e

        var guest = await _guestRepository.GetGuestAsync(guestId) ?? throw new NotFoundException(nameof(Guest), guestId);

        var review = _mapper.Map<Review>(request);

        review.Id = Guid.NewGuid();
        review.CreationDate = DateTime.UtcNow;
        review.LastModified = DateTime.UtcNow;

        review.HotelId = hotel.Id;
        review.GuestId = guestId;

        if (!await _guestRepository.HasGuestBookedHotelAsync(hotel, guest))
        {
            throw new BadRequestException("Guest has not booked this hotel");
        }

        if (await _guestRepository.HasGuestReviewedHotelAsync(hotel, guest))
        {
            throw new BadRequestException("Guest has already reviewed this hotel");
        }

        await _reviewRepository.AddReviewAsync(hotel, review);
        await _reviewRepository.SaveChangesAsync();
        return _mapper.Map<ReviewOutputModel>(review);
    }

    public async Task<ReviewOutputModel> GetReviewAsync(Guid id, Guid reviewId)
    {
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        var review = await _reviewRepository.GetReviewAsync(hotel, reviewId) ?? throw new NotFoundException(nameof(Review), reviewId);
        return _mapper.Map<ReviewOutputModel>(review);

    }

    public async Task<IEnumerable<ReviewOutputModel>> GetHotelReviewsAsync(Guid id)
    {
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        var reviews = await _reviewRepository.GetHotelReviewsAsync(hotel);
        return _mapper.Map<IEnumerable<ReviewOutputModel>>(reviews);
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
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);
        var rating = await _reviewRepository.GetHotelAverageRatingAsync(hotel);

        return double.Round(rating, 1); // round to 1 decimal place

    }
}
