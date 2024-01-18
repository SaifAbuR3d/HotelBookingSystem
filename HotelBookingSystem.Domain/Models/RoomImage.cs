namespace HotelBookingSystem.Domain.Models;

public class RoomImage : Entity
{
    public string ImageUrl { get; set; } = default!;
    public string? AlternativeText { get; set; }
    public Guid RoomId { get; set; }
}
