namespace HotelBookingSystem.Domain.Models;

public class HotelImage : Entity
{
    public string ImageUrl { get; set; } = default!;
    public string? AlternativeText { get; set; }
    public Guid HotelId { get; set; }
}
