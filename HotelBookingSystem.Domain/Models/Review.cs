namespace HotelBookingSystem.Domain.Models;

public class Review : Entity
{
    public Guest Guest { get; set; }
    public Hotel Hotel { get; set; }

    public Guid GuestId { get; set; }
    public Guid HotelId { get; set; }
    public string? Title { get; set; }
    public string Description { get; set; }

    public short Rating { get; set; } 
}
