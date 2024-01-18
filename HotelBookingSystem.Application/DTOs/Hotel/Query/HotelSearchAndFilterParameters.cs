using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Application.DTOs.Hotel.Query;

public class HotelSearchAndFilterParameters : ResourceQueryParameters
{
    /// <summary>
    /// Check in date in UTC (defaults to today)
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime CheckInDate { get; set; } = DateTime.UtcNow.Date;

    /// <summary>
    /// Check out date in UTC (defaults to tomorrow)
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime CheckOutDate { get; set; } = DateTime.UtcNow.Date.AddDays(1);

    /// <summary>
    /// Number of adults (defaults to 2)
    /// </summary>
    public int Adults { get; set; } = 2;

    /// <summary>
    /// Number of children (defaults to 0)
    /// </summary>
    public int Children { get; set; } = 0;

    /// <summary>
    /// Number of rooms (defaults to 1)
    /// </summary>
    public int Rooms { get; set; } = 1;

    // Additional filters for hotel search results page

    /// <summary>
    /// Minimum star rating filter
    /// </summary>
    public int? MinStarRating { get; set; }

    /// <summary>
    /// Maximum price filter
    /// </summary>
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// Minimum price filter
    /// </summary>
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// List of amenities to filter by
    /// </summary>
    public List<string>? Amenities { get; set; } = [];

    /// <summary>
    /// List of room types to filter by
    /// </summary>
    public List<RoomType>? RoomTypes { get; set; } = [];
}