namespace HotelBookingSystem.Application.DTOs.Hotel.Command;
/// <summary>
/// DTO for creating a hotel
/// </summary>
public class CreateHotelCommand
{
    /// <summary>
    /// Id of the city where the hotel is located
    /// </summary>
    public Guid CityId { get; set; }

    /// <summary>
    /// Name of the hotel
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Owner of the hotel
    /// </summary>>
    public string Owner { get; set; } = default!;

    /// <summary>
    /// Star Rate of the hotel
    /// </summary>
    public short StarRate { get; set; }

    /// <summary>
    /// Street of the hotel
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
    /// Check in time for the hotel (default is 14:00)
    /// </summary>
    public TimeOnly CheckInTime { get; set; } = Domain.Models.Hotel.DefaultCheckInTime;

    /// <summary>
    /// Check out time for the hotel (default is 11:00)
    /// </summary>
    public TimeOnly CheckOutTime { get; set; } = Domain.Models.Hotel.DefaultCheckOutTime;

}
