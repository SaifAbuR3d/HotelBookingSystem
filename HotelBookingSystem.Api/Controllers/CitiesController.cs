using HotelBookingSystem.Application.DTOs.City;
using HotelBookingSystem.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API endpoints for managing cities
/// </summary>>

[Route("api/[controller]")]
[ApiController]

public class CitiesController(ICityService cityService) : ControllerBase
{

    /// <summary>
    /// Get all cities
    /// </summary>
    /// <returns>All cities</returns>
    /// <response code="200">Returns all cities</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityOutputModel>>> GetAllCities()
    {
        var cities = await cityService.GetAllCitiesAsync();

        return Ok(cities);
    }

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
}
