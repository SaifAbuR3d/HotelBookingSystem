using HotelBookingSystem.Application.DTOs.Room.OutputModel;

namespace HotelBookingSystem.Application.DTOs.Hotel.OutputModel;

/// <summary>
/// This class is a data transfer object (DTO) for the <see cref="Domain.Models.Hotel"/> entity. Mainly for Hotel Page.
/// </summary>
public class HotelWithRoomsAndImagesOutputModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public short StarRate { get; set; }
    public string? Description { get; set; }

    public string CityName { get; set; } = default!;
    public string? Street { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // guest reviews

    public ICollection<HotelImageOutputModel> HotelImages { get; set; } = default!;  // 5 images maximum
    public ICollection<RoomWithImageOutputModel> Rooms { get; set; } = default!; // 10 rooms maximum
}
