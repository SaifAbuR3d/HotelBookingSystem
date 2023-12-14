namespace HotelBookingSystem.Domain.Models;

public class Guest : Entity
{
    public string Username { get; set; }
    public ICollection<Booking> Reservations { get; set; }
}
