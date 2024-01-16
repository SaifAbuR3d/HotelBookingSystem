using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.DTOs.Room.OutputModel;

public class RoomWithinInvoice
{
    public Guid Id { get; set; }
    public int RoomNumber { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public RoomType RoomType { get; set; }
    public decimal PricePerNight { get; set; }
    public decimal PricePerNightAfterDiscount { get; set; }
    public int NumberOfNights { get; set; }
    public decimal TotalRoomPrice { get; set; }
    public decimal TotalRoomPriceAfterDiscount { get; set; }
}
