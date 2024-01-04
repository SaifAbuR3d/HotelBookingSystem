using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Review.Command;
using HotelBookingSystem.Application.DTOs.Review.OutputModel;
using HotelBookingSystem.Application.DTOs.Review.Query;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API endpoints for managing hotels reviews
/// </summary>>

[Route("api/hotels")]
[ApiController]
public class ReviewsController(IReviewService reviewService) : ControllerBase
{

    /// <summary>
    /// Adds a review for a specific hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel for which the review is added.</param>
    /// <param name="request">The model containing review details.</param>
    /// <returns>The newly created Review</returns>
    /// <response code="201">Review successfully added.</response>
    /// <response code="400">Invalid input or missing required fields.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User is authenticated but doesn't have the necessary permissions.</response>
    /// <response code="404">Hotel with the specified id not found.</response>
    [HttpPost("{hotelId}/reviews")]
    public async Task<ActionResult> AddReview(Guid hotelId, CreateOrUpdateReviewCommand request)
    {
        var review = await reviewService.AddReviewAsync(hotelId, request);
        return CreatedAtAction(nameof(GetReview), new { hotelId = review.HotelId, reviewId = review.Id }, review);
    }


    /// <summary>
    /// Get a review by its id
    /// </summary>
    /// <param name="hotelId">The id of the hotel</param>
    /// <param name="reviewId">The id of the review</param>
    /// <returns>The review with the given id</returns>
    /// <response code="200">Returns the review with the given id</response>
    /// <response code="404">If the review is not found</response>
    [HttpGet("{hotelId}/reviews/{reviewId}", Name = "GetReview")]
    public async Task<ActionResult<ReviewOutputModel>> GetReview(Guid hotelId, Guid reviewId)
    {
        var review = await reviewService.GetReviewAsync(hotelId, reviewId);
        return Ok(review);
    }

    /// <summary>
    /// Get hotel average rating
    /// </summary>
    /// <param name="hotelId">The id of the hotel</param>
    /// <returns>Hotel average user ratings</returns>
    /// <response code="200">Returns hotel average rating</response>
    /// <response code="404">If the hotel is not found</response>
    [HttpGet("{hotelId}/reviews/average")]
    public async Task<ActionResult<double>> GetHotelAverageRating(Guid hotelId)
    {
        var rating = await reviewService.GetHotelAverageRatingAsync(hotelId);
        return Ok(new { rating });
    }


    /// <summary>
    /// Update a review
    /// </summary>
    /// <param name="hotelId">The id of the hotel having the review to update</param>
    /// <param name="reviewId">The id of the review to update</param>
    /// <param name="request">The data for the updated review</param>
    /// <returns>No content</returns>
    /// <response code="204">If the review is successfully updated</response>
    /// <response code="404">If the hotel or review is not found</response>
    /// <response code="400">If the request data is invalid</response>
    [HttpPut("{hotelId}/reviews/{reviewId}")]
    public async Task<ActionResult> UpdateReview(Guid hotelId, Guid reviewId, CreateOrUpdateReviewCommand request)
    {
        bool updated = await reviewService.UpdateReviewAsync(hotelId, reviewId, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }


    /// <summary>
    /// Delete a review
    /// </summary>
    /// <param name="hotelId">The id of the hotel having the review to delete</param>
    /// <param name="reviewId">The id of the review to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the review is successfully deleted</response>
    /// <response code="404">If the hotel is not found</response>
    [HttpDelete("{hotelId}/reviews/{reviewId}")]
    public async Task<ActionResult> DeleteReview(Guid hotelId, Guid reviewId)
    {
        bool deleted = await reviewService.DeleteReviewAsync(hotelId, reviewId);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }


    /// <summary>
    /// Get list of reviews for a hotel
    /// </summary>
    /// <param name="hotelId">The id of the hotel</param>
    /// <returns>List of reviews for the hotel</returns>
    /// <response code="200">Returns list of reviews for the hotel</response>
    /// <response code="404">If the hotel is not found</response>
    [HttpGet("{hotelId}/reviews", Name = "GetHotelReviews")]
    public async Task<ActionResult<ReviewOutputModel>> GetHotelReviews(Guid hotelId, [FromQuery] GetHotelReviewsQueryParameters request)
    {
        var (reviews, paginationMetadata) = await reviewService.GetHotelReviewsAsync(hotelId, request);

        AddPageLinks(paginationMetadata, request);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        return Ok(reviews);
    }

    private void AddPageLinks(PaginationMetadata paginationMetadata, ResourceQueryParameters parameters)
    {
        paginationMetadata.PreviousPageLink = paginationMetadata.HasPreviousPage ? CreatePageLink(paginationMetadata, parameters, next: false) : null;
        paginationMetadata.NextPageLink = paginationMetadata.HasNextPage ? CreatePageLink(paginationMetadata, parameters, next: true) : null;
    }

    private string? CreatePageLink(PaginationMetadata paginationMetadata, ResourceQueryParameters parameters, bool next)
    {
        var newPageNumber = next ? paginationMetadata.PageNumber + 1 : paginationMetadata.PageNumber - 1;
        return
            Url.Link("GetHotelReviews", new
            {
                sortOrder = parameters.SortOrder,
                sortColumn = parameters.SortColumn,
                pageNumber = newPageNumber,
                pageSize = paginationMetadata.PageSize,
                searchQuery = parameters.SearchTerm,
            });
    }
}
