using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.DTOs.Booking;

/// <summary>
/// This class is a data transfer object (DTO) for the <see cref="Domain.Models.Booking"/> entity.
/// </summary>
public class BookingConfirmationOutputModel
{
    public Guid Id { get; set; }
    public Guid GuestId { get; set; }
    public Guid RoomId { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int NumberOfAdults { get; set; }
    public int NumberOfChildren { get; set; }

    public string HotelName { get; set; } = default!;
    public RoomType RoomType { get; set; }
    public int RoomNumber { get; set; }
    public string? Street { get; set; } = default!;
    public decimal Price { get; set; }
}
