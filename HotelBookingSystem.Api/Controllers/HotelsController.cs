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
    /// Retrieves a list of hotels based on the specified query parameters.
    /// </summary>
    /// <remarks>
    /// The retrieval of hotels can be customized by providing various query parameters.
    /// These parameters include sorting options, page number, page size, and a search term.
    /// 
    /// Sample request:
    /// 
    ///     GET /hotels?sortOrder=asc&amp;sortColumn=name&amp;pageNumber=1&amp;pageSize=10&amp;searchQuery=Carlton
    ///     
    /// </remarks>
    /// <param name="request">The query parameters for hotel retrieval.</param>
    /// <returns>
    /// A collection of <see cref="HotelOutputModel"/> objects, each representing a hotel that matches the specified criteria.
    /// </returns>
    /// <response code="200">Returns the list of hotels based on the query parameters.</response>
    /// <response code="400">If the request parameters are invalid or missing.</response>

    [HttpGet(Name = "GetHotels")]
    public async Task<ActionResult<IEnumerable<HotelOutputModel>>> GetHotels([FromQuery] GetHotelsQueryParameters request)
    {
        var (hotels, paginationMetadata) = await hotelService.GetAllHotelsAsync(request);

        AddPageLinks(paginationMetadata, request);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        return Ok(hotels);
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
    /// a collection of <see cref="HotelSearchResultOutputModel"/> representing the search results.
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

    private void AddPageLinks(PaginationMetadata paginationMetadata, GetHotelsQueryParameters parameters)
    {
        paginationMetadata.PreviousPageLink = paginationMetadata.HasPreviousPage ? CreatePageLink(paginationMetadata, parameters, next: false) : null;
        paginationMetadata.NextPageLink = paginationMetadata.HasNextPage ? CreatePageLink(paginationMetadata, parameters, next: true) : null;
    }

    private string? CreatePageLink(PaginationMetadata paginationMetadata, GetHotelsQueryParameters parameters, bool next)
    {
        var newPageNumber = next ? paginationMetadata.PageNumber + 1 : paginationMetadata.PageNumber - 1;
        return
            Url.Link("GetHotels", new
            {
                sortOrder = parameters.SortOrder,
                sortColumn = parameters.SortColumn,
                pageNumber = newPageNumber,
                pageSize = paginationMetadata.PageSize,
                searchQuery = parameters.SearchTerm,
            });
    }


}