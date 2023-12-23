namespace HotelBookingSystem.Application.DTOs.Room;

/// <summary>
/// /// DTO for updating a Room
/// </summary>
public class UpdateRoomCommand
{
    /// <summary>
    /// Number of the room in the hotel
    /// </summary>
    public int RoomNumber { get; set; }

    /// <summary>
    /// Adults capacity of the room
    /// </summary>
    public int AdultsCapacity { get; set; }

    /// <summary>
    /// Children capacity of the room
    /// </summary>
    public int ChildrenCapacity { get; set; }
}
