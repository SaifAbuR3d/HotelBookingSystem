﻿using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Discount;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API endpoints for managing Discounts
/// </summary>
[Route("api/rooms")]
[ApiController]
public class DiscountsController(IDiscountService discountService) : ControllerBase
{
    /// <summary>
    /// Create a new discount
    /// </summary>
    /// <param name="roomId">The id of the room</param>
    /// <param name="command">The data for the new discount</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /rooms/{roomId}/discounts
    ///     {
    ///        "Percentage": 20
    ///        "StartDate": "2024-02-0200:00:00",
    ///        "EndDate": "2024-03-03T00:00:00"
    ///     }
    ///
    /// </remarks>
    /// <returns>The newly created discount</returns>
    /// <response code="201">Returns the newly created discount</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User is authenticated but doesn't have the necessary permissions.</response>
    /// <response code="400">If the request data is invalid</response>
    [HttpPost("{roomId}/discounts")]
    public async Task<ActionResult<DiscountOutputModel>> AddDiscount(Guid roomId, CreateDiscountCommand command)
    {
        var discount = await discountService.AddDiscountAsync(roomId, command);

        return CreatedAtAction(nameof(GetDiscount), new { roomId = discount.RoomId, id = discount.Id }, discount);
    }


    /// <summary>
    /// Get a discount by its id
    /// </summary>
    /// <param name="roomId">The id of the room</param>
    /// <param name="id">The id of the discount</param>
    /// <returns>The discount with the given id</returns>
    /// <response code="200">Returns the discount with the given id</response>
    /// <response code="404">If the discount is not found</response>
    [HttpGet("{roomId}/discounts/{id}", Name = "GetDiscount")]
    public async Task<ActionResult<DiscountOutputModel>> GetDiscount(Guid roomId, Guid id)
    {
        DiscountOutputModel discount = await discountService.GetDiscountAsync(roomId, id);

        return Ok(discount);
    }

    /// <summary>
    /// Delete a discount
    /// </summary>
    /// <param name="roomId">The id of the room</param>
    /// <param name="id">The id of the discount</param>
    /// <returns></returns>
    /// <response code="204">If the discount is deleted</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User is authenticated but doesn't have the necessary permissions.</response>
    /// <response code="404">If the discount is not found</response>
    [HttpDelete("{roomId}/discounts/{id}")]
    public async Task<ActionResult> DeleteDiscount(Guid roomId, Guid id)
    {
        bool deleted = await discountService.DeleteDiscountAsync(roomId, id);

        return NoContent();
    }


    /// <summary>
    /// Retrieves a collection of featured deals based on the specified count.
    /// </summary>
    /// <remarks>
    /// This endpoint allows clients to retrieve a curated list of featured deals.
    /// The response includes essential details for each deal,
    /// such as its hotel name, room type, star rating, discount percentage, and discounted price.
    /// 
    /// The number of featured deals to be retrieved can be specified
    /// using the <paramref name="deals"/> parameter. If no count is provided, the default is set to 5.
    /// 
    /// Sample request:
    /// 
    ///     GET /featured-deals?deals=3
    ///     
    /// </remarks>
    /// <param name="deals">The number of featured deals to retrieve. Default is 5.</param>
    /// <returns>
    /// A collection of <see cref="FeaturedDealOutputModel"/> objects, each representing a featured deal.
    /// </returns>
    /// <response code="200">Returns the collection of featured deals.</response>
    [HttpGet("featured-deals/{deals}")]
    public async Task<ActionResult<IEnumerable<FeaturedDealOutputModel>>> GetFeaturedDeals(int deals = 5)
    {
        var featuredDeals = await discountService.GetFeaturedDealsAsync(deals);

        return Ok(featuredDeals);
    }



}
