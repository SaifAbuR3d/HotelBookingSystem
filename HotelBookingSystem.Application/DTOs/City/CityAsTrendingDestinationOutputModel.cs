namespace HotelBookingSystem.Application.DTOs.City;

/// <summary>
/// This class is a data transfer object (DTO) for the <see cref="Domain.Models.City"/> entity.
/// </summary>
public class CityAsTrendingDestinationOutputModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Country { get; set; } = default!;
}
