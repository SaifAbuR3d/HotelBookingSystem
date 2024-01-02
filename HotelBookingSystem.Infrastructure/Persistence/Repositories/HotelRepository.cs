using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Hotel.Query;
using HotelBookingSystem.Domain.Models;
using HotelBookingSystem.Infrastructure.Persistence.Helpers;
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


    public async Task<(IEnumerable<Hotel>, PaginationMetadata)> GetAllHotelsAsync(GetHotelsQueryParameters request)
    {
        var query = _context.Hotels
            .Include(h => h.City)
            .Include(h => h.Rooms)
            .AsQueryable();

        SearchInNameOrDescription(ref query, request.SearchTerm);

        SortingHelper.ApplySorting(ref query, request.SortOrder, SortingHelper.GetHotelsSortingCriterion(request));

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query, request.PageNumber, request.PageSize);

        PaginationHelper.ApplyPagination(ref query, request.PageNumber, request.PageSize);

        var result = await query.ToListAsync();

        return (result, paginationMetadata);

    }

    public async Task<(IEnumerable<Hotel>, PaginationMetadata)> SearchAndFilterHotelsAsync(HotelSearchAndFilterParameters request)
    {
        var query = _context.Hotels
            .Include(h => h.Images)
            .Include(h => h.City)
            .Include(h => h.Reviews)
            .Include(h => h.Rooms).ThenInclude(r => r.Bookings)
            .AsSplitQuery()
            .AsQueryable();


        SearchInNameOrDescription(ref query, request.SearchTerm);

        FilterByRoomsAvailability(ref query, request.CheckInDate, request.CheckOutDate, request.Rooms);

        FilterByAdultsAndChildrenCapacity(ref query, request.Adults, request.Children);


        FilterByStarRating(ref query, request.MinStarRating);

        FilterByPrice(ref query, request.MinPrice, request.MaxPrice);

        FilterByAmenities(ref query, request.Amenities);

        FilterByRoomType(ref query, request.RoomTypes);


        SortingHelper.ApplySorting(ref query, request.SortOrder, SortingHelper.GetSearchResultsSortingCriterion(request));


        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query, request.PageNumber, request.PageSize);

        PaginationHelper.ApplyPagination(ref query, request.PageNumber, request.PageSize);

        var result = await query.ToListAsync(); 

        return (result, paginationMetadata);
    }


    private static void SearchInNameOrDescription(ref IQueryable<Hotel> hotels, string? searchQuery)
    {
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            hotels = hotels
                .Where(h => h.Name.Contains(searchQuery)
                            || (!string.IsNullOrWhiteSpace(h.Description) && h.Description.Contains(searchQuery)));
        }
    }

    private static void FilterByAdultsAndChildrenCapacity(ref IQueryable<Hotel> hotels, int adults, int children)
    {
            hotels = hotels
                     .Where(h => h.Rooms.Any(r => r.AdultsCapacity >= adults && r.ChildrenCapacity >= children));
    }

    private static void FilterByRoomsAvailability(ref IQueryable<Hotel> hotels,
        DateTime checkInDate, DateTime checkOutDate, int roomsCount)
    {

        var checkInDateAsDateOnly = DateOnly.FromDateTime((DateTime)checkInDate);
        var checkOutDateAsDateOnly = DateOnly.FromDateTime((DateTime)checkOutDate);

        hotels = hotels
                 .Where(h => h.Rooms.Count(r => !r.Bookings
                                  .Any(b => checkInDateAsDateOnly <= b.CheckOutDate && checkOutDateAsDateOnly >= b.CheckInDate)) >= roomsCount);
    }

    private static void FilterByStarRating(ref IQueryable<Hotel> hotels, int? minStarRating)
    {
        if (minStarRating.HasValue)
        {
            hotels = hotels.Where(h => h.StarRate >= minStarRating);
        }
    }

    private static void FilterByPrice(ref IQueryable<Hotel> hotels, decimal? minPrice, decimal? maxPrice)
    {
        if (minPrice.HasValue)
        {
            hotels = hotels.Where(h => h.Rooms.Any(r => r.Price >= minPrice));
        }

        if (maxPrice.HasValue)
        {
            hotels = hotels.Where(h => h.Rooms.Any(r => r.Price <= maxPrice));
        }
    }

    private static void FilterByRoomType(ref IQueryable<Hotel> hotels, List<RoomType>? roomTypes)
    {
        if (roomTypes != null && roomTypes.Count > 0)
        {
            hotels = hotels
                .Where(h => h.Rooms
                  .Any(r => roomTypes.Contains(r.RoomType)));
        }
    }

    private static void FilterByAmenities(ref IQueryable<Hotel> hotels, List<string>? amenities)
    {
        if (amenities != null && amenities.Count > 0)
        {
            hotels = hotels.Where(h => amenities.All(a => (!string.IsNullOrWhiteSpace(h.Description) && h.Description.Contains(a))));
        }
    }


}
