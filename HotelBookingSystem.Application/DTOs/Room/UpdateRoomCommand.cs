namespace HotelBookingSystem.Application.DTOs.Room;

public class UpdateRoomCommand
{
    public int RoomNumber { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
}
