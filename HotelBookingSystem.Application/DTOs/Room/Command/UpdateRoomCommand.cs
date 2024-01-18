namespace HotelBookingSystem.Application.DTOs.Room.Command;

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

    /// <summary>
    /// Price per night of the room
    /// </summary>
    public decimal Price { get; set; }
}
