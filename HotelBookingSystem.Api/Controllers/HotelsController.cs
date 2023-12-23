using HotelBookingSystem.Application.DTOs.Hotel;
using HotelBookingSystem.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API endpoints for managing hotels
/// </summary>>

[Route("api/[controller]")]
[ApiController]
public class HotelsController(IHotelService hotelService) : ControllerBase
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
    ///     /// Sample request:
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
    /// <param name="request">The data for the updated hotel</param>
    /// <returns>No content</returns>
    /// <response code="204">If the hotel is successfully updated</response>
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

}
