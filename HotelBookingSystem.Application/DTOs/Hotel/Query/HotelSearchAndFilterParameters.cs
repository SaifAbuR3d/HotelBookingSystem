using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Application.DTOs.Hotel.Query;

public class HotelSearchAndFilterParameters : ResourceQueryParameters
{

    [DataType(DataType.Date)]
    public DateTime CheckInDate { get; set; } = DateTime.UtcNow.Date;

    [DataType(DataType.Date)]
    public DateTime CheckOutDate { get; set; } = DateTime.UtcNow.Date.AddDays(1);

    public int Adults { get; set; } = 2;
    public int Children { get; set; } = 0;
    public int Rooms { get; set; } = 1;


    // Additional filters for hotel search results page
    public int? MinStarRating { get; set; }
    public decimal? MaxPrice { get; set; }
    public decimal? MinPrice { get; set; }
    public List<string>? Amenities { get; set; } = [];
    public List<RoomType>? RoomTypes { get; set; } = [];
}