namespace HotelBookingSystem.Application.DTOs.Booking;

/// <summary>
/// DTO for creating a Booking
/// </summary>
public class CreateBookingCommand 
{
    /// <summary>
    /// The Id of the desired Room
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// The Id of the Guest who is making the booking
    /// </summary>
    public Guid GuestId { get; set; }

    /// <summary>
    /// Number of adults staying in the room
    /// </summary>
    public int NumberOfAdults { get; set; }

    /// <summary>
    /// Number of children staying in the room
    /// </summary>
    public int NumberOfChildren { get; set; }

    /// <summary>
    /// Check-in date
    /// </summary>
    public DateOnly CheckInDate { get; set; }

    /// <summary>
    /// Check-out date
    /// </summary>
    public DateOnly CheckOutDate { get; set; }

    /// <summary>
    /// User special requests or remarks
    /// </summary>
    public string? UserRemarks { get; set; }

    /// <summary>
    /// Payment method (Cash, Credit Card, Bank Transfer)
    /// </summary>
    public string PaymentMethod { get; set; } = default!;

}
