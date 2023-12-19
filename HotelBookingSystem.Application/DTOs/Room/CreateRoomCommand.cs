using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.DTOs.Room;

public class CreateRoomCommand
{
    public string HotelName { get; set; }
    public int RoomNumber { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public RoomType RoomType { get; set; }
    public decimal Price { get; set; }
}