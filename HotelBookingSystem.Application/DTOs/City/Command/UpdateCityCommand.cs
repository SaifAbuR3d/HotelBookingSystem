﻿namespace HotelBookingSystem.Application.DTOs.City.Command;

/// <summary>
/// DTO for Updating a City 
/// </summary>
public class UpdateCityCommand
{
    /// <summary>
    /// Name of the City
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Country of the City
    /// </summary>
    public string Country { get; set; } = default!;

    /// <summary>
    /// Post Office of the City
    /// </summary>
    public string PostOffice { get; set; } = default!;
}
