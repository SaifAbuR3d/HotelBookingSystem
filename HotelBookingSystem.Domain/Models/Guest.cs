namespace HotelBookingSystem.Domain.Models;

public class Guest : Entity
{
    public string Username { get; set; } = default!;
    public ICollection<Booking> Bookings { get; set; }
}
