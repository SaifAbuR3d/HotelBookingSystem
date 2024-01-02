using AutoMapper;
using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Hotel.Command;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using HotelBookingSystem.Application.DTOs.Hotel.Query;
using HotelBookingSystem.Application.DTOs.Review;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace HotelBookingSystem.Application.Services;

public class HotelService(IHotelRepository hotelRepository,
                          ICityRepository cityRepository,
                          IGuestRepository guestRepository,
                          IMapper mapper,
                          IImageHandler imageHandler) : IHotelService 
{
    private readonly IHotelRepository _hotelRepository = hotelRepository;
    private readonly ICityRepository _cityRepository = cityRepository;
    private readonly IGuestRepository _guestRepository = guestRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IImageHandler _imageHandler = imageHandler;


    public async Task<IEnumerable<HotelOutputModel>> GetAllHotelsAsync()
    {
        var hotels = await _hotelRepository.GetAllHotelsAsync();
        var mapped = _mapper.Map<IEnumerable<HotelOutputModel>>(hotels);

        return mapped;
    }

    public async Task<HotelOutputModel?> GetHotelAsync(Guid id)
    {
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        var mapped = _mapper.Map<HotelOutputModel>(hotel);

        return mapped;
    }

    public async Task<bool> DeleteHotelAsync(Guid id)
    {
        bool deleted = await _hotelRepository.DeleteHotelAsync(id);
        if (deleted)
        {
            await _hotelRepository.SaveChangesAsync();
        }
        return deleted;
    }

    public async Task<HotelOutputModel> CreateHotelAsync(CreateHotelCommand request)
    {
        var city = await _cityRepository.GetCityByNameAsync(request.CityName) ?? throw new NotFoundException(nameof(City), request.CityName);

        var hotel = _mapper.Map<Hotel>(request);

        hotel.Id = Guid.NewGuid();
        hotel.CreationDate = DateTime.UtcNow;
        hotel.LastModified = DateTime.UtcNow;
        hotel.City = city;

        var createdHotel = await _hotelRepository.AddHotelAsync(hotel);
        await _hotelRepository.SaveChangesAsync();

        return _mapper.Map<HotelOutputModel>(createdHotel);
    }

    public async Task<bool> UpdateHotelAsync(Guid id, UpdateHotelCommand request)
    {
        var city = await _cityRepository.GetCityByNameAsync(request.CityName) ?? throw new NotFoundException(nameof(City), request.CityName);

        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        _mapper.Map(request, hotel);

        hotel.City = city;
        hotel.LastModified = DateTime.UtcNow;

        await _hotelRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UploadImageAsync(Guid hotelId, IFormFile file, string basePath, string? alternativeText, bool? thumbnail = false)
    { 
        if (string.IsNullOrWhiteSpace(basePath))
        {
            throw new BadFileException("an error occurred");
        }

        var hotel = await _hotelRepository.GetHotelAsync(hotelId) ?? throw new NotFoundException(nameof(Hotel), hotelId);
 
        var hotelDirectory = Path.Combine(basePath, "images", "hotels", hotelId.ToString());

        var uploadedImageUrl = await _imageHandler.UploadImage(file, hotelDirectory, thumbnail.GetValueOrDefault(false));

        var image = new HotelImage
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            ImageUrl = uploadedImageUrl,
            AlternativeText = alternativeText,
            HotelId = hotel.Id
        };

        await _hotelRepository.AddHotelImageAsync(hotel, image);
        await _hotelRepository.SaveChangesAsync();

        return true;
    }

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

        await _hotelRepository.AddReviewAsync(hotel, review);
        await _hotelRepository.SaveChangesAsync();

        return _mapper.Map<ReviewOutputModel>(review);

    }

    public async Task<ReviewOutputModel> GetReviewAsync(Guid id, Guid reviewId)
    {
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        var review = await _hotelRepository.GetReviewAsync(hotel, reviewId) ?? throw new NotFoundException(nameof(Review), reviewId);

        return _mapper.Map<ReviewOutputModel>(review);

    }

    public async Task<IEnumerable<ReviewOutputModel>> GetHotelReviewsAsync(Guid id)
    {
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        var reviews = await _hotelRepository.GetHotelReviewsAsync(hotel);

        return _mapper.Map<IEnumerable<ReviewOutputModel>>(reviews);
    }

    public async Task<bool> UpdateReviewAsync(Guid id, Guid reviewId, CreateOrUpdateReviewCommand request)
    {
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);

        var review = await _hotelRepository.GetReviewAsync(hotel, reviewId) ?? throw new NotFoundException(nameof(Review), reviewId);

        _mapper.Map(request, review);
        review.LastModified = DateTime.UtcNow;

        await _hotelRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteReviewAsync(Guid id, Guid reviewId)
    {
        bool deleted = await _hotelRepository.DeleteReviewAsync(id, reviewId);
        if (deleted)
        {
            await _hotelRepository.SaveChangesAsync();
        }
        return deleted;
    }

    public async Task<double> GetHotelAverageRatingAsync(Guid id)
    {
        var hotel = await _hotelRepository.GetHotelAsync(id) ?? throw new NotFoundException(nameof(Hotel), id);
        var rating = await _hotelRepository.GetHotelAverageRatingAsync(hotel);
     
        return double.Round(rating,1);

    }

    public async Task<(IEnumerable<HotelSearchResultOutputModel>, PaginationMetadata)> SearchAndFilterHotelsAsync(HotelSearchAndFilterParameters request)
    {
        var (hotels, paginationMetadata) = await _hotelRepository.SearchAndFilterHotelsAsync(request);

        var mapped = _mapper.Map<IEnumerable<HotelSearchResultOutputModel>>(hotels);

        return (mapped, paginationMetadata);
    }

}
