using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Hotel.Command;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using HotelBookingSystem.Application.DTOs.Hotel.Query;
using HotelBookingSystem.Application.DTOs.Review;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API endpoints for managing hotels
/// </summary>>

[Route("api/[controller]")]
[ApiController]
public class HotelsController(IHotelService hotelService, IWebHostEnvironment environment) : ControllerBase
{

    /// <summary>
    /// Get all hotels
    /// </summary>
    /// <returns>All hotels</returns>
    /// <response code="200">Returns all hotels</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HotelOutputModel>>> GetAllHotels()
    {
        var hotels = await hotelService.GetAllHotelsAsync();

        return Ok(hotels);
    }


    /// <summary>
    /// Get a hotel by its id
    /// </summary>
    /// <param name="id">The id of the hotel</param>
    /// <returns>The hotel with the given id</returns>
    /// <response code="200">Returns the hotel with the given id</response>
    /// <response code="404">If the hotel is not found</response>
    [HttpGet("{id}", Name = "GetHotel")]
    public async Task<ActionResult<HotelOutputModel>> GetHotel(Guid id)
    {
        var hotel = await hotelService.GetHotelAsync(id);

        return Ok(hotel);
    }


    /// <summary>
    /// Create a new hotel
    /// </summary>
    /// <param name="request">The data for the new hotel</param>
    /// <returns>The newly created hotel</returns>
    /// <remarks> 
    /// Sample request:
    ///
    ///     POST /hotels
    ///     {
    ///        "name": "Hotel Budapest",
    ///        "owner": "Hungarian Hotels Ltd.",
    ///        "location": "Budapest, 1054 Szabadság tér 9-10.",
    ///        "cityName": "Budapest"
    ///        "starRate" : "4"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created hotel</response>
    /// <response code="400">If the request data is invalid</response>
    [HttpPost]
    public async Task<ActionResult<HotelOutputModel>> CreateHotel(CreateHotelCommand request)
    {
        var hotel = await hotelService.CreateHotelAsync(request);

        return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
    }


    /// <summary>
    /// Delete a hotel
    /// </summary>
    /// <param name="id">The id of the hotel to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the hotel is successfully deleted</response>
    /// <response code="404">If the hotel is not found</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteHotel(Guid id)
    {
        var deleted = await hotelService.DeleteHotelAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }


    /// <summary>
    /// Update a hotel
    /// </summary>
    /// <param name="id">The id of the hotel to update</param>
    /// <param name="request">The data for the uploaded hotel</param>
    /// <returns>No content</returns>
    /// <response code="204">If the hotel is successfully uploaded</response>
    /// <response code="404">If the hotel is not found</response>
    /// <response code="400">If the request data is invalid</response>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateHotel(Guid id, UpdateHotelCommand request)
    {
        var updated = await hotelService.UpdateHotelAsync(id, request);
       
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }


    /// <summary>
    /// Upload an image to a hotel
    /// </summary>
    /// <param name="id">The id of the hotel to upload image</param>
    /// <param name="file">HotelImage data</param>
    /// <param name="alternativeText">Alternative Text(Alt)</param>
    /// <param name="thumbnail">indicates if the image should be used as thumbnail</param>
    /// <returns></returns>
    /// <response code="204">If the image is successfully uploaded</response>
    /// <response code="404">If the hotel is not found</response>
    /// <response code="400">If the request data is invalid</response>
    [HttpPost("{id}/images")]
    public async Task<ActionResult> UploadImage(Guid id, IFormFile file, string? alternativeText, bool? thumbnail = false)
    {
        var uploaded = await hotelService.UploadImageAsync(id, file, environment.WebRootPath, alternativeText, thumbnail);

        if (!uploaded)
        {
            return NotFound();
        }

        return NoContent();
    }


    /// <summary>
    /// Adds a review for a specific hotel.
    /// </summary>
    /// <param name="id">The ID of the hotel for which the review is added.</param>
    /// <param name="request">The model containing review details.</param>
    /// <returns>The newly created Review</returns>
    /// <response code="201">Review successfully added.</response>
    /// <response code="400">Invalid input or missing required fields.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User is authenticated but doesn't have the necessary permissions.</response>
    /// <response code="404">Hotel with the specified id not found.</response>
    [HttpPost("{id}/reviews")]
    public async Task<ActionResult> AddReview(Guid id, CreateOrUpdateReviewCommand request)
    {
        var review = await hotelService.AddReviewAsync(id, request);
        return CreatedAtAction(nameof(GetReview), new { id = review.HotelId, reviewId = review.Id }, review);
    }


    /// <summary>
    /// Get a review by its id
    /// </summary>
    /// <param name="id">The id of the hotel</param>
    /// <param name="reviewId">The id of the review</param>
    /// <returns>The review with the given id</returns>
    /// <response code="200">Returns the review with the given id</response>
    /// <response code="404">If the review is not found</response>
    [HttpGet("{id}/reviews/{reviewId}", Name = "GetReview")]
    public async Task<ActionResult<ReviewOutputModel>> GetReview(Guid id, Guid reviewId)
    {
        var review = await hotelService.GetReviewAsync(id, reviewId);
        return Ok(review);
    }


    /// <summary>
    /// Get list of reviews for a hotel
    /// </summary>
    /// <param name="id">The id of the hotel</param>
    /// <returns>List of reviews for the hotel</returns>
    /// <response code="200">Returns list of reviews for the hotel</response>
    /// <response code="404">If the hotel is not found</response>
    [HttpGet("{id}/reviews")]
    public async Task<ActionResult<ReviewOutputModel>> GetHotelReviews(Guid id)
    {
        var reviews = await hotelService.GetHotelReviewsAsync(id);
        return Ok(reviews);
    }

    /// <summary>
    /// Get hotel average rating
    /// </summary>
    /// <param name="id">The id of the hotel</param>
    /// <returns>Hotel average user ratings</returns>
    /// <response code="200">Returns hotel average rating</response>
    /// <response code="404">If the hotel is not found</response>
    [HttpGet("{id}/reviews/average")]
    public async Task<ActionResult<double>> GetHotelAverageRating(Guid id)
    {
        var rating = await hotelService.GetHotelAverageRatingAsync(id);
        return Ok(new {rating});
    }


    /// <summary>
    /// Update a review
    /// </summary>
    /// <param name="id">The id of the hotel having the review to update</param>
    /// <param name="reviewId">The id of the review to update</param>
    /// <param name="request">The data for the updated review</param>
    /// <returns>No content</returns>
    /// <response code="204">If the review is successfully updated</response>
    /// <response code="404">If the hotel or review is not found</response>
    /// <response code="400">If the request data is invalid</response>
    [HttpPut("{id}/reviews/{reviewId}")]
    public async Task<ActionResult> UpdateReview(Guid id, Guid reviewId, CreateOrUpdateReviewCommand request)
    {
        bool updated = await hotelService.UpdateReviewAsync(id, reviewId, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }


    /// <summary>
    /// Delete a review
    /// </summary>
    /// <param name="id">The id of the hotel having the review to delete</param>
    /// <param name="reviewId">The id of the review to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the review is successfully deleted</response>
    /// <response code="404">If the hotel is not found</response>
    [HttpDelete("{id}/reviews/{reviewId}")]
    public async Task<ActionResult> DeleteReview(Guid id, Guid reviewId)
    {
        bool deleted = await hotelService.DeleteReviewAsync(id, reviewId);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Searches and filters hotels based on the specified criteria.
    /// </summary>
    /// <remarks>
    /// The search can be performed by providing a query string along with optional parameters
    /// such as check-in and check-out dates, number of adults, children, and room count.
    /// Additional filters, including minimum star rating, maximum and minimum price,
    /// amenities, and room types, can be applied to narrow down the search results.
    /// 
    /// Sample request:
    ///
    ///     GET /hotels/search?query=Tokyo
    ///     
    /// </remarks>
    /// <param name="request">The query parameters for hotel search and filtering.</param>
    /// <returns>
    /// a list of <see cref="HotelSearchResultOutputModel"/> representing the search results.
    /// </returns>
    /// <response code="200">Returns the list of hotels based on the search criteria.</response>
    /// <response code="400">If the request parameters are invalid or missing.</response>
    [HttpGet("search", Name = "SearchHotels")]
    public async Task<ActionResult<IEnumerable<HotelSearchResultOutputModel>>> SearchAndFilterHotels([FromQuery] HotelSearchAndFilterParameters request)
    {
        var (hotels, paginationMetadata) = await hotelService.SearchAndFilterHotelsAsync(request);

        AddPageLinks(paginationMetadata, request); 

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        return Ok(hotels);
    }

    
    private void AddPageLinks(PaginationMetadata paginationMetadata, HotelSearchAndFilterParameters parameters)
    {
        paginationMetadata.PreviousPageLink = paginationMetadata.HasPreviousPage ? CreatePageLink(paginationMetadata, parameters, next: false) : null;
        paginationMetadata.NextPageLink = paginationMetadata.HasNextPage ? CreatePageLink(paginationMetadata, parameters, next: true) : null;
    }
     
    private string? CreatePageLink(PaginationMetadata paginationMetadata, HotelSearchAndFilterParameters parameters, bool next)
    {
        var newPageNumber = next ? paginationMetadata.PageNumber + 1 : paginationMetadata.PageNumber - 1;
        return
            Url.Link("SearchHotels", new
            {
                sortOrder = parameters.SortOrder,
                sortColumn = parameters.SortColumn,
                pageNumber = newPageNumber,
                pageSize = paginationMetadata.PageSize,
                searchQuery = parameters.SearchTerm,

                checkInDate = parameters.CheckInDate,
                checkOutDate = parameters.CheckOutDate,
                adults = parameters.Adults,
                children = parameters.Children,
                rooms = parameters.Rooms,

                minStarRating = parameters.MinStarRating,
                maxPrice = parameters.MaxPrice,
                minPrice = parameters.MinPrice,
                amenities = parameters?.Amenities,
                roomTypes = parameters?.RoomTypes
            });
    }
}