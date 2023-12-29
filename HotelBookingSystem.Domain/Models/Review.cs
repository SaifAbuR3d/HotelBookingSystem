namespace HotelBookingSystem.Domain.Models;

public class Review : Entity
{
    public Guest Guest { get; set; } = default!;
    public Hotel Hotel { get; set; } = default!;

    public Guid GuestId { get; set; }
    public Guid HotelId { get; set; }
    public string? Title { get; set; }
    public string Description { get; set; } = default!;

    public short Rating { get; set; } 
}
