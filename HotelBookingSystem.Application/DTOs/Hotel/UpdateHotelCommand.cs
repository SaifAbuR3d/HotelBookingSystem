﻿namespace HotelBookingSystem.Application.DTOs.Hotel;
/// <summary>
/// DTO for creating a hotel
/// </summary>
public class UpdateHotelCommand
{
    /// <summary>
    /// Name of the hotel
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Owner of the hotel
    /// </summary>>
    public string Owner { get; set; }

    /// <summary>
    /// Location of the hotel
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// City name where the hotel is located
    /// </summary>
    public string CityName { get; set; }
}