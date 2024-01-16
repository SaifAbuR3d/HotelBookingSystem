using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Hotel.Command;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using HotelBookingSystem.Application.DTOs.Hotel.Query;
using HotelBookingSystem.Application.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API endpoints for managing hotels
/// </summary>>

[Authorize(Policy = Policies.AdminOnly)]
[Route("api/[controller]")]
[ApiController]
public class HotelsController(IHotelService hotelService,
                              IWebHostEnvironment environment, 
                              ILogger<HotelsController> logger) : ControllerBase
{

    /// <summary>
    /// Get a hotel by its id
    /// </summary>
    /// <param name="id">The id of the hotel</param>
    /// <returns>The hotel with the given id</returns>
    /// <response code="200">Returns the hotel with the given id</response>
    /// <response code="404">If the hotel is not found</response>
    [AllowAnonymous]
    [HttpGet("{id}", Name = "GetHotel")]
    public async Task<ActionResult<HotelOutputModel>> GetHotel(Guid id)
    {
        logger.LogInformation("GetHotel started for hotel with ID: {HotelId}", id);

        var hotel = await hotelService.GetHotelAsync(id);

        logger.LogInformation("GetHotel for hotel with ID: {HotelId} completed successfully", id);
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
    ///        "street": "King lu, 19 st."
    ///        "latitude": "15.9",
    ///        "longitude": "20.5",
    ///        "cityId": "{cityId}"
    ///        "starRate" : "4"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created hotel</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized (not an admin)</response> 
    [HttpPost]
    public async Task<ActionResult<HotelOutputModel>> CreateHotel(CreateHotelCommand request)
    {
        logger.LogInformation("CreateHotel started for request: {@CreateHotel}", request);

        var hotel = await hotelService.CreateHotelAsync(request);

        logger.LogInformation("CreateHotel for request: {@CreateHotel} completed successfully", request);
        return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
    }


    /// <summary>
    /// Delete a hotel
    /// </summary>
    /// <param name="id">The id of the hotel to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the operation is successfully done</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized (not an admin)</response> 
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteHotel(Guid id)
    {
        logger.LogInformation("DeleteHotel started for hotel with ID: {HotelId}", id);

        await hotelService.DeleteHotelAsync(id);

        logger.LogInformation("DeleteHotel for hotel with ID: {HotelId} completed successfully", id);
        return NoContent();
    }


    /// <summary>
    /// Update a hotel
    /// </summary>
    /// <param name="id">The id of the hotel to update</param>
    /// <param name="request">The data for the uploaded hotel</param>
    /// <returns>No content</returns>
    /// <remarks> 
    /// Sample request:
    ///
    ///     PUT /hotels/{hotelId}
    ///     {
    ///        "name": "Hotel Budapest",
    ///        "owner": "Hungarian Hotels Ltd.",
    ///        "street": "King lu, 19 st."
    ///        "latitude": "15.9",
    ///        "longitude": "20.5",
    ///        "starRate" : "4"
    ///        "cityId": "{cityId}"
    ///     }
    ///
    /// </remarks>
    /// <response code="204">If the hotel is successfully uploaded</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized (not an admin)</response> 
    /// <response code="404">If the hotel is not found</response>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateHotel(Guid id, UpdateHotelCommand request)
    {
        logger.LogInformation("UpdateHotel started for hotel with ID: {HotelId}, request: {@UpdateHotel} ", id, request);

        await hotelService.UpdateHotelAsync(id, request);

        logger.LogInformation("UpdateHotel completed successfully for hotel with ID: {HotelId}, request: {@UpdateHotel}", id, request);
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
    /// <remarks> 
    /// Sample request:
    ///
    ///     PUT /hotels/{hotelId}
    ///     {
    ///        "name": "Hotel Budapest",
    ///        "owner": "Hungarian Hotels Ltd.",
    ///        "street": "King lu, 19 st."
    ///        "latitude": "15.9",
    ///        "longitude": "20.5",
    ///        "cityId": "{cityId}"
    ///     }
    ///
    /// </remarks>
    /// <response code="204">If the image is successfully uploaded</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized (not an admin)</response> 
    /// <response code="404">If the hotel is not found</response>
    [HttpPost("{id}/images")]
    public async Task<ActionResult> UploadImage(Guid id, IFormFile file, string? alternativeText, bool? thumbnail = false)
    {
        logger.LogInformation("UploadImage started for hotel with ID: {HotelId}", id);

        await hotelService.UploadImageAsync(id, file, environment.WebRootPath, alternativeText, thumbnail);

        logger.LogInformation("UploadImage completed successfully for hotel with ID: {HotelId}", id);
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
    [AllowAnonymous]
    [HttpGet(Name = "GetHotels")]
    public async Task<ActionResult<IEnumerable<HotelOutputModel>>> GetHotels([FromQuery] GetHotelsQueryParameters request)
    {
        logger.LogInformation("GetHotels started for query: {@GetHotelsQuery}", request);

        var (hotels, paginationMetadata) = await hotelService.GetAllHotelsAsync(request);

        AddPageLinks(paginationMetadata, request);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        logger.LogInformation("GetHotels for query: {@GetHotelsQuery} completed successfully", request);
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
    ///     GET /hotels/search?searchTerm=Tokyo&amp;maxPrice=100&amp;minStarRating=4&amp;amenities=FreeWifi&amp;roomTypes=Single
    ///     
    /// </remarks>
    /// <param name="request">The query parameters for hotel search and filtering.</param>
    /// <returns>
    /// a collection of <see cref="HotelSearchResultOutputModel"/> representing the search results.
    /// </returns>
    /// <response code="200">Returns the list of hotels based on the search criteria.</response>
    /// <response code="400">If the request parameters are invalid or missing.</response>
    [AllowAnonymous]
    [HttpGet("search", Name = "SearchHotels")]
    public async Task<ActionResult<IEnumerable<HotelSearchResultOutputModel>>> SearchAndFilterHotels([FromQuery] HotelSearchAndFilterParameters request)
    {
        logger.LogInformation("SearchAndFilterHotels started for query: {@HotelSearchAndFilterParameters}", request);

        var (hotels, paginationMetadata) = await hotelService.SearchAndFilterHotelsAsync(request);

        AddPageLinks(paginationMetadata, request);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        logger.LogInformation("SearchAndFilterHotels for query: {@HotelSearchAndFilterParameters} completed successfully", request);
        return Ok(hotels);
    }

    
    private void AddPageLinks(PaginationMetadata paginationMetadata, HotelSearchAndFilterParameters parameters)
    {
        logger.LogDebug("AddPageLinks started for query: {@parameters}, with pagination metadata: {@paginationMetadata}", parameters, paginationMetadata);

        paginationMetadata.PreviousPageLink = paginationMetadata.HasPreviousPage ? CreatePageLink(paginationMetadata, parameters, next: false) : null;
        paginationMetadata.NextPageLink = paginationMetadata.HasNextPage ? CreatePageLink(paginationMetadata, parameters, next: true) : null;

        logger.LogDebug("AddPageLinks for query: {@parameters}, with pagination metadata: {@paginationMetadata} completed successfully", parameters, paginationMetadata);

    }

    private string? CreatePageLink(PaginationMetadata paginationMetadata, HotelSearchAndFilterParameters parameters, bool next)
    {
        if (next)
        {
            logger.LogDebug("CreatePageLinks for the next page started for query: {@parameters}, with pagination metadata: {@paginationMetadata}", parameters, paginationMetadata);
        }
        else
        {
            logger.LogDebug("CreatePageLinks for the previous page started for query: {@parameters}, with pagination metadata: {@paginationMetadata}", parameters, paginationMetadata);
        }

        var newPageNumber = next ? paginationMetadata.PageNumber + 1 : paginationMetadata.PageNumber - 1;
        var link =
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

        return link; 
    }

    private void AddPageLinks(PaginationMetadata paginationMetadata, GetHotelsQueryParameters parameters)
    {
        logger.LogDebug("AddPageLinks started for query: {@parameters}, with pagination metadata: {@paginationMetadata}", parameters, paginationMetadata);

        paginationMetadata.PreviousPageLink = paginationMetadata.HasPreviousPage ? CreatePageLink(paginationMetadata, parameters, next: false) : null;
        paginationMetadata.NextPageLink = paginationMetadata.HasNextPage ? CreatePageLink(paginationMetadata, parameters, next: true) : null;

        logger.LogDebug("AddPageLinks for query: {@parameters}, with pagination metadata: {@paginationMetadata} completed successfully", parameters, paginationMetadata);

    }

    private string? CreatePageLink(PaginationMetadata paginationMetadata, GetHotelsQueryParameters parameters, bool next)
    {
        if (next)
        {
            logger.LogDebug("CreatePageLinks for the next page started for query: {@parameters}, with pagination metadata: {@paginationMetadata}", parameters, paginationMetadata);
        }
        else
        {
            logger.LogDebug("CreatePageLinks for the previous page started for query: {@parameters}, with pagination metadata: {@paginationMetadata}", parameters, paginationMetadata);
        }

        var newPageNumber = next ? paginationMetadata.PageNumber + 1 : paginationMetadata.PageNumber - 1;
        var link =
            Url.Link("GetHotels", new
            {
                sortOrder = parameters.SortOrder,
                sortColumn = parameters.SortColumn,
                pageNumber = newPageNumber,
                pageSize = paginationMetadata.PageSize,
                searchQuery = parameters.SearchTerm,
            });

        return link; 
    }


}