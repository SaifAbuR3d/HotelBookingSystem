namespace HotelBookingSystem.Application.DTOs.Hotel.OutputModel;

/// <summary>
/// This class is a data transfer object (DTO) for the <see cref="Domain.Models.Hotel"/> entity. Mainly for Admin Page.
/// </summary>

public class HotelOutputModel
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
    public string Name { get; set; } = default!;
    public string Owner { get; set; } = default!;
    public short StarRate { get; set; }
    public int RoomsNumber { get; set; }
}
