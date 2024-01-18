namespace HotelBookingSystem.Application.DTOs.Hotel.OutputModel;

public class HotelSearchResultOutputModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int StarRate { get; set; }
    public HotelImageOutputModel HotelImage { get; set; } = default!;
    public string? Description { get; set; }
    public decimal PriceStartingFrom { get; set; }
}
