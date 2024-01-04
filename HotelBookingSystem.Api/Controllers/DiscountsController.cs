using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
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
    [HttpPost("{roomId}/discounts")]
    public async Task<ActionResult<DiscountOutputModel>> AddDiscount(Guid roomId, CreateDiscountCommand command)
    {
        var discount = await discountService.AddDiscountAsync(roomId, command);

        return CreatedAtAction(nameof(GetDiscount), new { roomId = discount.RoomId, id = discount.Id }, discount);
    }

    [HttpGet("{roomId}/discounts/{id}", Name = "GetDiscount")]
    public async Task<ActionResult<DiscountOutputModel>> GetDiscount(Guid roomId, Guid id)
    {
        DiscountOutputModel discount = await discountService.GetDiscountAsync(roomId, id);

        return Ok(discount);
    }

    [HttpDelete("{roomId}/discounts/{id}")]
    public async Task<ActionResult> DeleteDiscount(Guid roomId, Guid id)
    {
        bool deleted = await discountService.DeleteDiscountAsync(roomId, id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("featured-deals/{deals}")]
    public async Task<ActionResult<IEnumerable<FeaturedDealOutputModel>>> GetFeaturedDeals(int deals = 5)
    {
        var featuredDeals = await discountService.GetFeaturedDealsAsync(deals);

        return Ok(featuredDeals);
    }


}
