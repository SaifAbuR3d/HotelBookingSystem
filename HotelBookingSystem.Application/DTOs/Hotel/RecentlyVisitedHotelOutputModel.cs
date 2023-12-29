namespace HotelBookingSystem.Application.DTOs.Hotel;

/// <summary>
/// This class is a data transfer object (DTO) for the <see cref="Domain.Models.Hotel"/> entity. Mainly for Recently Visited Hotels feature.
/// </summary>
public class RecentlyVisitedHotelOutputModel
{
    public string HotelName { get; set; } = default!;
    public HotelImageOutputModel HotelImage { get; set; } = default!;  
    public string CityName { get; set; } = default!;
    public int StarRating { get; set; }
    public decimal Price { get; set; }

    //  more pricing info
}
