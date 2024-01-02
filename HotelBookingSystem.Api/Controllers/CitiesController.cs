using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.City.Command;
using HotelBookingSystem.Application.DTOs.City.OutputModel;
using HotelBookingSystem.Application.DTOs.City.Query;
using HotelBookingSystem.Application.DTOs.Common;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API endpoints for managing cities
/// </summary>>

[Route("api/[controller]")]
[ApiController]

public class CitiesController(ICityService cityService, IWebHostEnvironment environment) : ControllerBase
{
    /// <summary>
    /// Get a city by its id
    /// </summary>
    /// <param name="id">The id of the city</param>
    /// <returns>The city with the given id</returns>
    /// <response code="200">Returns the city with the given id</response>
    /// <response code="404">If the city is not found</response>
    [HttpGet("{id}", Name = "GetCity")]
    public async Task<ActionResult<CityOutputModel>> GetCity(Guid id)
    {
        var city = await cityService.GetCityAsync(id);

        return Ok(city);
    }

    /// <summary>
    /// Create a new city
    /// </summary>
    /// <param name="request">The data for the new city</param>
    /// <returns>The newly created city</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /cities
    ///     {
    ///        "name": "Budapest",
    ///        "country": "Hungary",
    ///        "postOffice": "1054"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created city</response>
    /// <response code="400">If the request data is invalid</response>
    [HttpPost]
    public async Task<ActionResult<CityOutputModel>> CreateCity(CreateCityCommand request)
    {
        var city = await cityService.CreateCityAsync(request);

        return CreatedAtAction(nameof(GetCity), new { id = city.Id }, city);
    }

    /// <summary>
    /// Delete a city
    /// </summary>
    /// <param name="id">The id of the city to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the city is deleted</response>
    /// <response code="404">If the city is not found</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCity(Guid id)
    {
        var deleted = await cityService.DeleteCityAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Update a city
    /// </summary>
    /// <param name="id">The id of the city to update</param>
    /// <param name="request">The data for the updated city</param>
    /// <returns>No content</returns>
    /// <response code="204">If the city is updated</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="404">If the city is not found</response>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCity(Guid id, UpdateCityCommand request)
    {
        var updated = await cityService.UpdateCityAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Returns TOP N most visited cities, N is 5 by default
    /// </summary>
    /// <param name="count">The number of trending destinations to retrieve. Default is 5.</param>
    /// <returns>
    /// An asynchronous task representing the operation, returning an <see cref="ActionResult"/> containing a collection
    /// of <see cref="CityAsTrendingDestinationOutputModel"/> objects, each representing a trending destination city.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This endpoint allows clients to retrieve a curated list of trending destinations, specifically the topmost visited cities.
    /// The response includes essential details for each city, such as its unique identifier, name, and a visually appealing thumbnail.
    /// </para>
    /// <para>
    /// The number of trending destinations to be retrieved can be specified using the <paramref name="count"/> parameter.
    /// If no count is provided, the default is set to 5.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Retrieves the top 3 trending destinations
    /// var trendingDestinations = await MostVisitedCities(3);
    /// // Process the trendingDestinations ActionResult as needed.
    /// </code>
    /// </example>
    /// <response code="200">Returns TOP N most visited cities, N is 5 by default</response>
    /// <seealso cref="CityAsTrendingDestinationOutputModel"/>
    [HttpGet("trending-destinations")]
    public async Task<ActionResult<IEnumerable<CityAsTrendingDestinationOutputModel>>> MostVisitedCities(int count = 5)
    {
        var cities = await cityService.MostVisitedCitiesAsync(count);

        return Ok(cities);
    }

    /// <summary>
    /// Upload an image to a city
    /// </summary>
    /// <param name="id">The id of the city to upload image</param>
    /// <param name="file">HotelImage data</param>
    /// <param name="alternativeText">Alternative Text(Alt)</param>
    /// <param name="thumbnail">indicates if the image should be used as thumbnail</param>
    /// <returns></returns>
    /// <response code="204">If the image is successfully uploaded</response>
    /// <response code="404">If the city is not found</response>
    /// <response code="400">If the request data is invalid</response>
    /// 
    [HttpPost("{id}/images")]
    public async Task<ActionResult> UploadImage(Guid id, IFormFile file, string? alternativeText, bool? thumbnail = false)
    {
        var uploaded = await cityService.UploadImageAsync(id, file, environment.WebRootPath, alternativeText, thumbnail);

        if (!uploaded)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get all cities
    /// </summary>
    /// <returns>All cities</returns>
    /// <response code="200">Returns all cities</response>
    [HttpGet(Name = "GetCities")]
    public async Task<ActionResult<IEnumerable<CityOutputModel>>> GetAllCities([FromQuery] GetCitiesQueryParameters request)
    {
        var (cities, paginationMetadata) = await cityService.GetAllCitiesAsync(request);

        AddPageLinks(paginationMetadata, request); 

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        return Ok(cities);
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
            Url.Link("GetCities", new
            {
                sortOrder = parameters.SortOrder,
                sortColumn = parameters.SortColumn,
                pageNumber = newPageNumber,
                pageSize = paginationMetadata.PageSize,
                searchQuery = parameters.SearchTerm,
            });
    }

}