using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Review.Query;
using HotelBookingSystem.Domain.Models;
using HotelBookingSystem.Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Persistence.Repositories;

public class ReviewRepository(ApplicationDbContext context) : IReviewRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Review> AddReviewAsync(Hotel hotel, Review review)
    {
        await _context.Reviews.AddAsync(review);
        hotel.Reviews.Add(review);
        return review;
    }

    public async Task<Review?> GetReviewAsync(Hotel hotel, Guid reviewId)
    {
        return await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId && r.HotelId == hotel.Id);
    }

    public async Task<bool> DeleteReviewAsync(Guid id, Guid reviewId)
    {
        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId && r.HotelId == id);
        if (review is null)
        {
            return false;
        }

        _context.Reviews.Remove(review);
        return true;
    }

    public async Task<(IEnumerable<Review>, PaginationMetadata)> GetHotelReviewsAsync(Hotel hotel, GetHotelReviewsQueryParameters request)
    {
        var query = _context.Reviews
                        .Include(r => r.Hotel).Include(r => r.Guest)
                        .Where(r => r.HotelId == hotel.Id);

        SearchInTitleOrDescription(ref query, request.SearchTerm);

        SortingHelper.ApplySorting(ref query, request.SortOrder, SortingHelper.GetReviewsSortingCriterion(request));

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query, request.PageNumber, request.PageSize);

        PaginationHelper.ApplyPagination(ref query, request.PageNumber, request.PageSize);

        var result = await query.ToListAsync();

        return (result, paginationMetadata);

    }

    private static void SearchInTitleOrDescription(ref IQueryable<Review> query, string? searchTerm)
    {
        if(!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(r => !string.IsNullOrEmpty(r.Title) && r.Title.Contains(searchTerm) 
                             || r.Description.Contains(searchTerm));
        }
    }

    public async Task<double> GetHotelAverageRatingAsync(Hotel hotel)
    {
        return await _context.Reviews
            .Where(r => r.HotelId == hotel.Id)
            .AverageAsync(r => r.Rating);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 1;
    }

}
