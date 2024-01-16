namespace HotelBookingSystem.Domain.Models;

public class Booking : Entity
{
    public Booking(Guest guest, Hotel hotel, List<Room> rooms)
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
        Guest = guest;
        Rooms = rooms;
        Hotel = hotel;
    }

    public Booking()
    {
        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }

    // price at time of booking (may differ than current price of the room)
    public decimal Price { get; set; }
    public int NumberOfAdults { get; set; }
    public int NumberOfChildren { get; set; }

    // Multiple rooms can be booked at once
    public ICollection<Room> Rooms { get; set; } = new List<Room>(); 
    public Guest Guest { get; set; } = default!;
    public Guid GuestId { get; set; }

    // Booking is made for a specific hotel (All rooms should be in the same hotel)
    public Hotel Hotel { get; set; } = default!;
    public Guid HotelId { get; set; }

}
