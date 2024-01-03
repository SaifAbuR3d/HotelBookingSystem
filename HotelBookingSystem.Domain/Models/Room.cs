namespace HotelBookingSystem.Domain.Models;

public class Room : Entity
{
    public int RoomNumber { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public ICollection<RoomImage> Images { get; set; } = new List<RoomImage>();
    public RoomType RoomType { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public Hotel Hotel { get; set; } = default!;
    public Guid HotelId { get; set; }
    public ICollection<Discount> Discounts { get; set; } = new List<Discount>();
}
