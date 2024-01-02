namespace HotelBookingSystem.Application.DTOs.City.OutputModel;

/// <summary>
/// This class is a data transfer object (DTO) for the <see cref="Domain.Models.City"/> entity. Mainly for Trending Destinations Feature.
/// </summary>
public class CityAsTrendingDestinationOutputModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Country { get; set; } = default!;
    public CityImageOutputModel CityImage { get; set; } = default!;
}
