using HotelBookingSystem.Application.DTOs.Hotel;
using HotelBookingSystem.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// Controller for managing guests
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GuestsController(IGuestService guestService) : ControllerBase
{

    /// <summary>
    /// Gets recently booked rooms for a guest
    /// </summary>
    /// <param name="guestId"></param>
    /// <param name="count"></param>
    /// <returns>the last 5 different hotels the guest visited</returns>
    /// <response code = "200">Returns the last 5 different hotels the guest visited</response>>
    /// <response code="404">If the guest is not found</response>
    [HttpGet("{guestId}/recently-visited-hotels")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<RecentlyVisitedHotelOutputModel>>> GetRecentlyVisitedHotels(Guid guestId, int count = 5)
    {
        return Ok(await guestService.GetRecentlyVisitedHotelsAsync(guestId, count));
    }
}
