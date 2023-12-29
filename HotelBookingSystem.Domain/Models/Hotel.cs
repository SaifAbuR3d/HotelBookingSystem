namespace HotelBookingSystem.Domain.Models;

public class Hotel : Entity
{
    public string Name { get; set; } = default!;
    public string Owner { get; set; } = default!;
    public short StarRate { get; set; }
    public string? Street { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Description { get; set; }

    public TimeOnly CheckInTime { get; set; }
    public TimeOnly CheckOutTime { get; set; }

    public ICollection<HotelImage> Images { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<Room> Rooms { get; set; }


    public City City { get; set; }
    public Guid CityId { get; set; }
    public int RoomsNumber => Rooms.Count;

}
