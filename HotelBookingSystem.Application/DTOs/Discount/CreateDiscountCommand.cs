namespace HotelBookingSystem.Application.DTOs.Discount;

public class CreateDiscountCommand
{
    public Guid RoomId { get; set; }

    // the user must supply either a percentage or a discounted price (this is validated with FluentValidation)
    public double? Percentage { get; set; }
    public decimal? DiscountedPrice { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
