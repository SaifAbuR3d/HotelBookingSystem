using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Booking;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API endpoints for managing bookings
/// </summary>>

[Route("api/[controller]")]
[ApiController]
public class BookingsController(IBookingService bookingService, 
                                ILogger<BookingsController> logger) : ControllerBase
{
    /// <summary>
    /// Get a booking by its id
    /// </summary>
    /// <param name="id">The id of the booking</param>
    /// <returns>The booking with the given id</returns>
    /// <response code="200">Returns the booking with the given id</response>
    /// <response code="404">If the booking is not found</response>
    [HttpGet("{id}", Name = "GetBookingAsync")]
    public async Task<ActionResult<BookingConfirmationOutputModel>> GetBooking(Guid id)
    {
        logger.LogInformation("GetBooking started for booking with ID: {BookingId}", id);

        var booking = await bookingService.GetBookingAsync(id);

        logger.LogInformation("GetBooking for booking with ID: {BookingId} completed successfully", id);
        return Ok(booking);
    }

    /// <summary>
    /// Create a new booking
    /// </summary>
    /// <param name="request">The data for the new booking</param>
    /// <returns>The newly created booking</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /booking
    ///     {
    ///        "guestId": "a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11",
    ///        "roomId": "31d26773-2eb9-4695-bc61-0b717bd97e0b",
    ///        "checkInDate": "2024-01-10",
    ///        "checkOutDate": "2024-01-15",
    ///        "numberOfAdults": 1,
    ///        "numberOfChildren": 1,
    ///        "userRemarks": "I need a baby cot.",
    ///        "paymentMethod": "CreditCard"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created booking</response>
    /// <response code = "404">If the hotel/room or guest is not found</response>
    /// <response code="400">If the request data is invalid</response>
    [HttpPost]
    public async Task<ActionResult<BookingConfirmationOutputModel>> CreateBooking(CreateBookingCommand request)
    {
        logger.LogInformation("CreateBooking started for request: {@CreateBooking}", request);

        var booking = await bookingService.CreateBookingAsync(request);

        logger.LogInformation("CreateBooking for request: {@CreateBooking} completed successfully", request);
        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
    }


    /// <summary>
    /// Delete a booking
    /// </summary>
    /// <param name="id">The id of the booking to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the booking is deleted</response>
    /// <response code="404">If the booking is not found</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(Guid id)
    {
        logger.LogInformation("DeleteBooking started for booking with ID: {BookingId}", id);

        await bookingService.DeleteBookingAsync(id);

        logger.LogInformation("DeleteBooking for booking with ID: {BookingId} completed successfully", id);
        return NoContent();
    }
}
