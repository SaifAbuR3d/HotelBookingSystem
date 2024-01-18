using HotelBookingSystem.Application.DTOs.Guest;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.DTOs.Review.OutputModel;

public class ReviewOutputModel
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string Description { get; set; } = default!;
    public int Rating { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
    public GuestOutputModel Guest { get; set; } = default!;
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = default!;
}
