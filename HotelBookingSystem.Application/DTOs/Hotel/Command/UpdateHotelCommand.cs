﻿namespace HotelBookingSystem.Application.DTOs.Hotel.Command;
/// <summary>
/// DTO for creating a hotel
/// </summary>
public class UpdateHotelCommand
{
    /// <summary>
    /// Name of the hotel
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Owner of the hotel
    /// </summary>>
    public string Owner { get; set; } = default!;

    /// <summary>
    /// Location of the hotel
    /// </summary>
    public string Street { get; set; } = default!;

    /// <summary>
    /// Latitude for the hotel  
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude for the hotel
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// City name where the hotel is located
    /// </summary>
    public string CityName { get; set; } = default!;
}
