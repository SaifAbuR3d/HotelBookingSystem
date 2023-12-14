namespace HotelBookingSystem.Domain.Models;

public class Hotel : Entity
{
    public string Name { get; set; }
    public string Owner { get; set; }
    public short StarRate { get; set; }
    public ICollection<Room> Rooms { get; set; }
    public string Location { get; set; }
    public string? Description { get; set; }
    public ICollection<HotelImage> Images { get; set; }
    public ICollection<Review> Reviews { get; set; }

    public TimeOnly CheckInTime { get; set; }
    public TimeOnly CheckOutTime { get; set; }

    public City City { get; set; }
    public Guid CityId { get; set; }
    public int RoomsNumber => Rooms.Count;

}
