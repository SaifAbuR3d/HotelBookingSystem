namespace HotelBookingSystem.Application.DTOs.Hotel;

public class RecentlyVisitedHotelOutputModel
{
    // thumbnail image

    public string HotelName { get; set; } = default!;
    public string CityName { get; set; } = default!;
    public int StarRating { get; set; }
    public decimal Price { get; set; }

    //  more pricing info

}
