using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using HotelBookingSystem.Application.DTOs.Room.OutputModel;

namespace HotelBookingSystem.Application.DTOs.Booking.OutputModel;

/// <summary>
/// This class is a data transfer object (DTO) for the <see cref="Domain.Models.Booking"/> entity.
/// </summary>
public class Invoice
{
    public Guid ConfirmationId { get; set; } // Booking Id
    public decimal TotalPrice { get; set; }
    public decimal TotalPriceAfterDiscount { get; set; }
    public List<RoomWithinInvoice> Rooms { get; set; } = default!;
    public HotelWithinInvoice Hotel { get; set; } = default!;
    public Guid GuestId { get; set; }
    public string GuestFullName { get; set; } = default!;
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int NumberOfAdults { get; set; }
    public int NumberOfChildren { get; set; }
}
