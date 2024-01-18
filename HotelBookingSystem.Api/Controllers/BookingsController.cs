using Asp.Versioning;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Booking.Command;
using HotelBookingSystem.Application.DTOs.Booking.OutputModel;
using HotelBookingSystem.Application.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API endpoints for managing bookings
/// </summary>>

[ApiVersion("1.0")]
[Authorize]
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
    public async Task<ActionResult<BookingOutputModel>> GetBooking(Guid id)
    {
        logger.LogInformation("GetBooking started for booking with ID: {BookingId}", id);

        var bookingInvoice = await bookingService.GetBookingAsync(id);

        logger.LogInformation("GetBooking for booking with ID: {BookingId} completed successfully", id);
        return Ok(bookingInvoice);
    }


    /// <summary>
    /// Get an invoice by booking id
    /// </summary>
    /// <param name="id">The booking id</param>
    /// <returns>The invoice with the given id</returns>
    /// <response code="200">Returns the invoice with the given id</response>
    /// <response code="404">If there is no booking related to the given id</response>
    [HttpGet("{id}/invoice")]
    public async Task<ActionResult<Invoice>> GetInvoice(Guid id)
    {
        logger.LogInformation("GetInvoice started for booking with ID: {BookingId}", id);

        var invoice = await bookingService.GetInvoiceAsync(id); 

        logger.LogInformation("GetInvoice for booking with ID: {BookingId} completed successfully", id);
        return Ok(invoice);
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
    ///        "roomsId": ["31d26773-2eb9-4695-bc61-0b717bd97e0b", "54a26773-2eb9-4695-bc61-0b717bd97e0b"],
    ///        "hotelId": "46c26773-2eb9-4695-bc61-0b717bd97e0b",
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
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized</response>
    /// <response code="404">If the hotel/room or guest is not found</response>
    [Authorize(Policy = Policies.GuestOnly)]
    [HttpPost]
    public async Task<ActionResult<BookingOutputModel>> CreateBooking(CreateBookingCommand request)
    {
        logger.LogInformation("CreateBooking started for request: {@CreateBooking}", request);

        var booking = await bookingService.CreateBookingAsync(request);

        logger.LogInformation("CreateBooking for request: {@CreateBooking} completed successfully", request);
        return CreatedAtAction(nameof(GetBooking), new { id = booking.ConfirmationId }, booking);
    }


    /// <summary>
    /// Delete a booking
    /// </summary>
    /// <param name="id">The id of the booking to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the operation is successfully done</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized</response>
    [Authorize(Policy = Policies.GuestOnly)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(Guid id)
    {
        logger.LogInformation("DeleteBooking started for booking with ID: {BookingId}", id);

        await bookingService.DeleteBookingAsync(id);

        logger.LogInformation("DeleteBooking for booking with ID: {BookingId} completed successfully", id);
        return NoContent();
    }

    /// <summary>
    /// Retrieve a booking invoice as a PDF file, to download or print
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{id}/pdf")]
    public async Task<FileContentResult> GetInvoicePdf(Guid id)
    {
        var pdfBytes = await bookingService.GetInvoicePdfByBookingIdAsync(id);

        return File(pdfBytes, "application/pdf", "invoice.pdf");
    }
}
