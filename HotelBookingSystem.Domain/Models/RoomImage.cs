namespace HotelBookingSystem.Domain.Models;

public class RoomImage : Entity
{
    public byte[] ImageData { get; set; }
    public string Format { get; set; }
    public Guid RoomId { get; set; }
}
