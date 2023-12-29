using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.DTOs.Room;
/// <summary>
/// This class is a data transfer object (DTO) for the <see cref="Domain.Models.Room"/> entity. Mainly for Hotel Page.
/// </summary>
public class RoomWithImageOutputModel
{
    public Guid Id { get; set; }
    public RoomType RoomType { get; set; }
    public int RoomNumber { get; set;}
    public decimal Price { get; set;}
    public string? Description { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public RoomImageOutputModel RoomImage { get; set; } = default!;
}
