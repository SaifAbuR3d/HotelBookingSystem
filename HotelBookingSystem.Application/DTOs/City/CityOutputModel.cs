namespace HotelBookingSystem.Application.DTOs.City;

/// <summary>
/// This class is a data transfer object (DTO) for the <see cref="Domain.Models.City"/> entity.
/// </summary>

public class CityOutputModel
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
    public string Name { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string PostOffice { get; set; } = default!;

    public int NumberOfHotels { get; set; }
}
