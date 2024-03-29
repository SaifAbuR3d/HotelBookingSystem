﻿using Asp.Versioning;
using HotelBookingSystem.Api.Helpers;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.City.Command;
using HotelBookingSystem.Application.DTOs.City.OutputModel;
using HotelBookingSystem.Application.DTOs.City.Query;
using HotelBookingSystem.Application.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API endpoints for managing cities
/// </summary>>

[ApiVersion("1.0")]
[Authorize(Policy = Policies.AdminOnly)]
[Route("api/[controller]")]
[ApiController]
public class CitiesController(ICityService cityService,
                              IWebHostEnvironment environment,
                              ILogger<CitiesController> logger) : ControllerBase
{

    /// <summary>
    /// Get a city by its id
    /// </summary>
    /// <param name="id">The id of the city</param>
    /// <returns>The city with the given id</returns>
    /// <response code="200">Returns the city with the given id</response>
    /// <response code="404">If the city is not found</response>
    [AllowAnonymous]
    [HttpGet("{id}", Name = "GetCity")]
    public async Task<ActionResult<CityOutputModel>> GetCity(Guid id)
    {
        logger.LogInformation("GetCity started for city with ID: {CityId}", id);

        var city = await cityService.GetCityAsync(id);

        logger.LogInformation("GetCity for city with ID: {CityId} completed successfully", id);
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
    ///        "postOffice": "10543"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created city</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized (not an admin)</response>
    [HttpPost]
    public async Task<ActionResult<CityOutputModel>> CreateCity(CreateCityCommand request)
    {
        logger.LogInformation("CreateCity started for request: {@CreateCity}", request);

        var city = await cityService.CreateCityAsync(request);

        logger.LogInformation("CreateCity for request: {@CreateCity} completed successfully", request);

        return CreatedAtAction(nameof(GetCity), new { id = city.Id }, city);
    }

    /// <summary>
    /// Delete a city
    /// </summary>
    /// <param name="id">The id of the city to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the operation is successfully done</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized (not an admin)</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCity(Guid id)
    {
        logger.LogInformation("DeleteCity started for city with ID: {CityId}", id);

        await cityService.DeleteCityAsync(id);

        logger.LogInformation("DeleteCity for city with ID: {CityId} completed successfully", id); 
        return NoContent();
    }

    /// <summary>
    /// Update a city
    /// </summary>
    /// <param name="id">The id of the city to update</param>
    /// <param name="request">The data for the updated city</param>
    /// <returns>No content</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /cities/{cityId}
    ///     {
    ///        "name": "Budapest",
    ///        "country": "Hungary",
    ///        "postOffice": "11176"
    ///     }
    ///
    /// </remarks>
    /// <response code="204">If the city is updated</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized (not an admin)</response>
    /// <response code="404">If the city is not found</response>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCity(Guid id, UpdateCityCommand request)
    {
        logger.LogInformation("UpdateCity started for city with ID: {CityId}, request: {@UpdateCity} ", id, request);

        var updated = await cityService.UpdateCityAsync(id, request);

        logger.LogInformation("UpdateCity completed successfully for city with ID: {CityId}, request: {@UpdateCity} ", id, request);
        return NoContent();
    }

    /// <summary>
    /// Returns TOP N most visited cities, N is 5 by default
    /// </summary>
    /// <param name="count">The number of trending destinations to retrieve. Default is 5.</param>
    /// <remarks>
    /// This endpoint allows clients to retrieve a curated list of trending destinations, specifically the topmost visited cities.
    /// The response includes essential details for each city, such as its unique identifier,
    /// name, and a visually appealing thumbnail.
    /// 
    /// The number of trending destinations to be retrieved can be specified using the <paramref name="count"/> parameter.
    /// If no count is provided, the default is set to 5.
    /// 
    /// Sample request:
    /// 
    ///     GET /trending-destinations?count=3
    ///     
    /// </remarks>
    /// <returns>
    /// a collection of <see cref="CityAsTrendingDestinationOutputModel"/> objects, each representing a trending destination city.
    /// </returns>
    /// <response code="200">Returns TOP N most visited cities, N is 5 by default</response>
    /// <seealso cref="CityAsTrendingDestinationOutputModel"/>
    [AllowAnonymous]
    [HttpGet("trending-destinations")]
    public async Task<ActionResult<IEnumerable<CityAsTrendingDestinationOutputModel>>> MostVisitedCities(int count = 5)
    {
        logger.LogInformation("Retrieving top {Count} most visited cities has started", count); 

        var cities = await cityService.MostVisitedCitiesAsync(count);

        logger.LogInformation("Retrieving top {Count} most visited cities completed successfully", count);
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
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized (not an admin)</response>
    /// <response code="404">If the city is not found</response>
    [HttpPost("{id}/images")]
    public async Task<ActionResult> UploadImage(Guid id, IFormFile file, string? alternativeText, bool? thumbnail = false)
    {
        logger.LogInformation("UploadImage started for city with ID: {CityId}", id);

        await cityService.UploadImageAsync(id, file, environment.WebRootPath, alternativeText, thumbnail);

        logger.LogInformation("UploadImage completed successfully for city with ID: {CityId}", id);

        return NoContent();
    }

    /// <summary>
    /// Retrieves a list of cities based on the specified query parameters.
    /// </summary>
    /// <remarks>
    /// The retrieval of cities can be customized by providing various query parameters.
    /// These parameters include sorting options, page number, page size, and a search term.
    /// 
    /// Sample request:
    /// 
    ///     GET /cities?sortOrder=asc&amp;sortColumn=name
    ///     
    /// </remarks>
    /// <param name="request">The query parameters for city retrieval.</param>
    /// <returns>
    /// a collection of <see cref="CityOutputModel"/> objects, each representing a city that matches the specified criteria.
    /// </returns>
    /// <response code="200">Returns the list of cities based on the query parameters.</response>
    /// <response code="400">If the request parameters are invalid or missing.</response>
    [AllowAnonymous]
    [HttpGet(Name = "GetCities")]
    public async Task<ActionResult<IEnumerable<CityOutputModel>>> GetCities([FromQuery] GetCitiesQueryParameters request)
    {
        logger.LogInformation("GetCities started for query: {@GetCitiesQuery}", request);

        var (cities, paginationMetadata) = await cityService.GetAllCitiesAsync(request);

        PageLinker.AddPageLinks(Url, nameof(GetCities), paginationMetadata, request);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        logger.LogInformation("GetCities for query: {@GetCitiesQuery} completed successfully", request);
        return Ok(cities);
    }

}