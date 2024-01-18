using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.DTOs.Room.Command;

/// <summary>
/// /// DTO for creating a Room
/// </summary>
public class CreateRoomCommand
{
    /// <summary>
    /// Id of the hotel where the room is located
    /// </summary>
    public Guid HotelId { get; set; }

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
    /// Type of the room (Standard, Luxury, Budget, Boutique)
    /// </summary>
    public RoomType RoomType { get; set; }

    /// <summary>
    /// Current price of the room
    /// </summary>
    public decimal Price { get; set; }
}