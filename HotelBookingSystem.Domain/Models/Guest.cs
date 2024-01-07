namespace HotelBookingSystem.Domain.Models;

public class Guest : Entity
{
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
