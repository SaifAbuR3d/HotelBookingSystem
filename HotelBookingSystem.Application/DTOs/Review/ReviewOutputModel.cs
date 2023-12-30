namespace HotelBookingSystem.Application.DTOs.Review;

public class ReviewOutputModel
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string Description { get; set; } = default!;
    public int Rating { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }

    public Guid GuestId { get; set; } // add more guest info (name,maybe visit date..)
    public Guid HotelId { get; set; } // add more hotel info (maybe name,..)
}
