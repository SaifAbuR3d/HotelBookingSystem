namespace HotelBookingSystem.Domain.Models;

public class Room : Entity
{
    public int RoomNumber { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public ICollection<RoomImage> Images { get; set; }
    public bool IsAvailable { get; set; }
    public RoomType RoomType { get; set; }

    public Hotel Hotel { get; set; }
    public Guid HotelId { get; set; }
}
