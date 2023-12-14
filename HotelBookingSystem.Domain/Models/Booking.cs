namespace HotelBookingSystem.Domain.Models;

public class Booking : Entity
{
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public decimal Price { get; set; } // price at time of booking (may differ than current price of the room) 
    public Room Room { get; set; }
    public Guid RoomId { get; set; }
    public Guest Guest { get; set; }
    public Guid GuestId { get; set; }
}
