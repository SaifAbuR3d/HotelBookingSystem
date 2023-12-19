namespace HotelBookingSystem.Application.DTOs.Room;

public class RoomOutputModel
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
    public int RoomNumber { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public bool IsAvailable { get; set; }
    public string HotelName { get; set; }
}
