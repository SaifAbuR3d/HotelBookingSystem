using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Application.DTOs.Booking.Command;

/// <summary>
/// DTO for creating a Booking
/// </summary>
public class CreateBookingCommand
{
    /// <summary>
    /// List of Id's of the desired Rooms
    /// </summary>
    public List<Guid> RoomIds { get; set; } = default!;

    /// <summary>
    /// the desired Hotel Id 
    /// </summary>
    public Guid HotelId { get; set; }

    /// <summary>
    /// Number of adults staying in the room
    /// </summary>
    public int NumberOfAdults { get; set; }

    /// <summary>
    /// Number of children staying in the room
    /// </summary>
    public int NumberOfChildren { get; set; }

    /// <summary>
    /// Check-in date in UTC
    /// </summary>
    public DateTime CheckInDate { get; set; }

    /// <summary>
    /// Check-out date in UTC
    /// </summary>
    public DateTime CheckOutDate { get; set; }

    /// <summary>
    /// User special requests or remarks
    /// </summary>
    public string? UserRemarks { get; set; }

    /// <summary>
    /// Payment method (Cash, Credit Card, Bank Transfer)
    /// </summary>
    public string PaymentMethod { get; set; } = default!;

}
