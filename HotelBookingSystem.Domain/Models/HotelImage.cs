namespace HotelBookingSystem.Domain.Models;

public class HotelImage : Entity
{
    public byte[] ImageData { get; set; }
    public string Format { get; set; }
    public Guid HotelId { get; set; }
}
