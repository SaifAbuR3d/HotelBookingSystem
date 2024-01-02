namespace HotelBookingSystem.Application.DTOs.Room.OutputModel;

/// <summary>
/// This class is a data transfer object (DTO) for the <see cref="Domain.Models.Room"/> entity. Mainly for Admin Page.
/// </summary>
public class RoomOutputModel
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
    public int RoomNumber { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public string HotelName { get; set; } = default!;
}
