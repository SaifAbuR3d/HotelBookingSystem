using HotelBookingSystem.Application.DTOs.Room.OutputModel;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.DTOs.Booking.OutputModel;

public class BookingOutputModel
{
    public Guid ConfirmationId { get; set; } // Booking Id
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public decimal Price { get; set; }
    public int NumberOfAdults { get; set; }
    public int NumberOfChildren { get; set; }
    public ICollection<int> RoomNumbers { get; set; } = new List<int>();
    public string GuestFullName { get; set; } = default!;
    public Guid GuestId { get; set; }
    public string HotelName { get; set; } = default!;
    public Guid HotelId { get; set; }
}
