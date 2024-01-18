using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.DTOs.Hotel.OutputModel;

public class FeaturedDealOutputModel
{
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = default!;
    public HotelImageOutputModel HotelImage { get; set; } = default!;
    public int StarRate { get; set; }
    public string? Street { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Guid RoomId { get; set; }
    public int RoomNumber { get; set; }
    public RoomType RoomType { get; set; }
    public decimal OriginalPrice { get; set; }

    public decimal DiscountedPrice { get; set; }
    public double DiscountPercentage { get; set;}

    public string CityName { get; set; } = default!;
    public string Country { get; set; } = default!;
}
