namespace HotelBookingSystem.Domain.Models;

public class CityImage : Entity
{
    public string ImageUrl { get; set; } = default!;
    public string? AlternativeText { get; set; }

    public Guid CityId { get; set; }
}
