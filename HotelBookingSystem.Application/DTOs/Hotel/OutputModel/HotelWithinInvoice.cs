namespace HotelBookingSystem.Application.DTOs.Hotel.OutputModel;

public class HotelWithinInvoice
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string CityName { get; set; } = default!;
    public string? Street { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
