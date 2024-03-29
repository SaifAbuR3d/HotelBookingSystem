﻿using Asp.Versioning;
using HotelBookingSystem.Api.Helpers;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Review.Command;
using HotelBookingSystem.Application.DTOs.Review.OutputModel;
using HotelBookingSystem.Application.DTOs.Review.Query;
using HotelBookingSystem.Application.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API endpoints for managing hotels reviews
/// </summary>>

[ApiVersion("1.0")]
[Authorize(Policy = Policies.GuestOnly)]
[Route("api/hotels")]
[ApiController]
public class ReviewsController(IReviewService reviewService, 
                               ILogger<ReviewsController> logger) : ControllerBase
{

    /// <summary>
    /// Adds a review for a specific hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel for which the review is added.</param>
    /// <param name="request">The model containing review details.</param>
    /// <remarks>
    /// This endpoint allows users to submit a review for a particular hotel identified by the provided <paramref name="hotelId"/>.
    /// 
    /// Sample request:
    /// 
    ///     POST /hotels/{hotelId}/reviews
    ///     {
    ///         "Title": "Amazing Experience",
    ///         "Description": "The hotel provided exceptional service and comfortable accommodations.",
    ///         "Rating": 5
    ///     }
    ///     
    /// </remarks>
    /// <returns>The newly created Review</returns>
    /// <response code="201">Review successfully added.</response>
    /// <response code="400">Invalid input or missing required fields.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User is not authorized (not a guest, or didn't visit the hotel).</response>
    /// <response code="404">Hotel with the specified id not found.</response>
    [HttpPost("{hotelId}/reviews")]
    public async Task<ActionResult> AddReview(Guid hotelId, CreateOrUpdateReviewCommand request)
    {
        logger.LogInformation("AddReview started for hotel with ID: {HotelId}, request: {@AddReview}", hotelId, request);

        var review = await reviewService.AddReviewAsync(hotelId, request);

        logger.LogInformation("AddReview for hotel with ID: {HotelId}, request: {@AddReview} completed successfully",
            hotelId, request);
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
    [AllowAnonymous]
    [HttpGet("{hotelId}/reviews/{reviewId}", Name = "GetReview")]
    public async Task<ActionResult<ReviewOutputModel>> GetReview(Guid hotelId, Guid reviewId)
    {
        logger.LogInformation("GetReview started for hotel with ID: {HotelId}, review with ID: {ReviewId}", hotelId, reviewId); 

        var review = await reviewService.GetReviewAsync(hotelId, reviewId);

        logger.LogInformation("GetReview for hotel with ID: {HotelId}, review with ID: {ReviewId} completed successfully", hotelId, reviewId);
        return Ok(review);
    }

    /// <summary>
    /// Get hotel average rating
    /// </summary>
    /// <param name="hotelId">The id of the hotel</param>
    /// <returns>Hotel average user ratings</returns>
    /// <response code="200">Returns hotel average rating</response>
    /// <response code="404">If the hotel is not found</response>
    [AllowAnonymous]
    [HttpGet("{hotelId}/reviews/average")]
    public async Task<ActionResult<double>> GetHotelAverageRating(Guid hotelId)
    {
        logger.LogInformation("GetHotelAverageRating started for hotel with ID: {HotelId}", hotelId);

        var rating = await reviewService.GetHotelAverageRatingAsync(hotelId);

        logger.LogInformation("GetHotelAverageRating for hotel with ID: {HotelId} completed successfully", hotelId);
        return Ok(new { rating });
    }


    /// <summary>
    /// Update a review
    /// </summary>
    /// <param name="hotelId">The id of the hotel having the review to update</param>
    /// <param name="reviewId">The id of the review to update</param>
    /// <param name="request">The data for the updated review</param>
    /// <returns>No content</returns>
    /// <remarks>
    /// 
    /// Sample request:
    /// 
    ///     PUT /hotels/{hotelId}/reviews/{reviewId}
    ///     {
    ///         "Title": "Good Experience",
    ///         "Description": "The hotel provided good service and comfortable accommodations.",
    ///         "Rating": 4
    ///     }
    ///     
    /// </remarks>
    /// <response code="204">If the review is successfully updated</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User is not authorized (not a guest, or didn't visit the hotel).</response>
    /// <response code="404">If the hotel or review is not found</response>
    [HttpPut("{hotelId}/reviews/{reviewId}")]
    public async Task<ActionResult> UpdateReview(Guid hotelId, Guid reviewId, CreateOrUpdateReviewCommand request)
    {
        logger.LogInformation("UpdateReview started for hotel with ID: {HotelId}, review with ID: {ReviewId}, request: {@UpdateReview}", hotelId, reviewId, request);

        await reviewService.UpdateReviewAsync(hotelId, reviewId, request);

        logger.LogInformation("UpdateReview for hotel with ID: {HotelId}, review with ID: {ReviewId}, request: {@UpdateReview} completed successfully", hotelId, reviewId, request);
        return NoContent();
    }


    /// <summary>
    /// Delete a review
    /// </summary>
    /// <param name="hotelId">The id of the hotel having the review to delete</param>
    /// <param name="reviewId">The id of the review to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the operation is successfully done</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User is not authorized (not a guest, or didn't visit the hotel).</response>
    [HttpDelete("{hotelId}/reviews/{reviewId}")]
    public async Task<ActionResult> DeleteReview(Guid hotelId, Guid reviewId)
    {
        logger.LogInformation("DeleteReview started for hotel with ID: {HotelId}, review with ID: {ReviewId}", hotelId, reviewId);

        await reviewService.DeleteReviewAsync(hotelId, reviewId);

        logger.LogInformation("DeleteReview for hotel with ID: {HotelId}, review with ID: {ReviewId} completed successfully", hotelId, reviewId);
        return NoContent();
    }


    /// <summary>
    /// Retrieves the reviews for a specific hotel based on the specified query parameters.
    /// </summary>
    /// <remarks>
    /// The retrieval of hotel reviews can be customized by providing various query parameters.
    /// These parameters include sorting options, page number, page size, and a search term.
    /// 
    /// Sample request:
    /// 
    ///     GET /hotels/{hotelId}/reviews?sortOrder=desc&amp;sortColumn=creationDate&amp;pageNumber=1&amp;pageSize=5&amp;searchQuery=Excellent
    ///     
    /// </remarks>
    /// <param name="hotelId">The unique identifier of the hotel.</param>
    /// <param name="request">The query parameters for hotel review retrieval.</param>
    /// <returns>
    /// A collection of <see cref="ReviewOutputModel"/> objects, each representing a review that matches the specified criteria for the specified hotel.
    /// </returns>
    /// <response code="200">Returns the list of hotel reviews based on the query parameters.</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="404">If the hotel is not found.</response>
    [AllowAnonymous]
    [HttpGet("{hotelId}/reviews", Name = "GetHotelReviews")]
    public async Task<ActionResult<ReviewOutputModel>> GetHotelReviews(Guid hotelId, [FromQuery] GetHotelReviewsQueryParameters request)
    {
        logger.LogInformation("GetHotelReviews started for hotel with ID: {HotelId}, request: {@GetHotelReviews}", hotelId, request);

        var (reviews, paginationMetadata) = await reviewService.GetHotelReviewsAsync(hotelId, request);

        PageLinker.AddPageLinks(Url, nameof(GetHotelReviews), paginationMetadata, request);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        logger.LogInformation("GetHotelReviews for hotel with ID: {HotelId}, request: {@GetHotelReviews} completed successfully", hotelId, request);
        return Ok(reviews);
    }
}
