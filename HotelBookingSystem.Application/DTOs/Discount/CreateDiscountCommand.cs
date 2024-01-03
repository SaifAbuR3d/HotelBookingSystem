namespace HotelBookingSystem.Application.DTOs.Discount;

public class CreateDiscountCommand
{
    // the user must supply either a percentage or a discounted price (this is validated with FluentValidation)
    public decimal? Percentage { get; set; }
    public decimal? DiscountedPrice { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
