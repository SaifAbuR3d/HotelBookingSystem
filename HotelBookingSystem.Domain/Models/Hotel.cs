namespace HotelBookingSystem.Domain.Models;
using static HotelBookingSystem.Domain.Constants.City;
public class Hotel : Entity
{
    public string Name { get; set; } = default!;
    public string Owner { get; set; } = default!;
    public short StarRate { get; set; }
    public string? Street { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Description { get; set; }
    
    public TimeOnly CheckInTime { get; set; } = DefaultCheckInTime;
    public TimeOnly CheckOutTime { get; set; } = DefaultCheckOutTime;

    public ICollection<HotelImage> Images { get; set; } = new List<HotelImage>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Room> Rooms { get; set; } = new List<Room>();


    public City City { get; set; } = default!;
    public Guid CityId { get; set; }
    public int RoomsNumber => Rooms.Count;

    public readonly static TimeOnly DefaultCheckInTime = new(14, 0);
    public readonly static TimeOnly DefaultCheckOutTime = new(11, 0);

}
