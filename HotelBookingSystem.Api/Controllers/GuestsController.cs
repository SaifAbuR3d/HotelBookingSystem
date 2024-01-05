using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// Controller for managing guests
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GuestsController(IGuestService guestService, 
                              ILogger<GuestsController> logger) : ControllerBase
{

    /// <summary>
    /// Retrieves a collection of unique recently visited hotels for a guest, presenting essential details.
    /// </summary>
    /// <param name="guestId">The id of the guest for whom recently visited hotels are to be retrieved.</param>
    /// <param name="count">The maximum number of unique recently visited hotels to retrieve. Default is 5.</param>
    /// <remarks>
    /// The resulting collection provides essential information about the last N different hotels the guest visited, such as hotel name, city name, star rating, and price.
    /// 
    /// Sample request:
    /// 
    ///     GET guests/{guestId}/recently-visited-hotels?count=3
    ///     
    /// </remarks>
    /// <returns>
    /// A collection of <see cref="RecentlyVisitedHotelOutputModel"/> objects, each representing an hotel the user recently visited
    /// </returns>
    /// <response code="200">Returns the last 5 different hotels the guest visited</response>>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User is authenticated but doesn't have the necessary permissions.</response>
    /// <response code="404">If the guest is not found</response>
    [HttpGet("{guestId}/recently-visited-hotels")]
    public async Task<ActionResult<IEnumerable<RecentlyVisitedHotelOutputModel>>> GetRecentlyVisitedHotels(Guid guestId, int count = 5)
    {
        logger.LogInformation("GetRecentlyVisitedHotels started for guest with ID: {GuestId}, count: {recentlyVisitedHotelsCount}", guestId, count); 

        var hotels = await guestService.GetRecentlyVisitedHotelsAsync(guestId, count);

        logger.LogInformation("GetRecentlyVisitedHotels for guest with ID: {GuestId}, count: {recentlyVisitedHotelsCount} completed successfully", guestId, count);
        return Ok(hotels);
    }
}
