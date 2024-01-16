namespace HotelBookingSystem.Application.DTOs.Discount;

/// <summary>
/// DTO for creating a discount
/// </summary>
public class CreateDiscountCommand
{
    // the user must supply either a percentage or a discounted price (this is validated with FluentValidation)

    /// <summary>
    /// Percentage of the discount for the room
    /// </summary>
    public decimal? Percentage { get; set; }

    /// <summary>
    /// Discounted price for the room
    /// </summary>
    public decimal? DiscountedPrice { get; set; }

    /// <summary>
    /// Discount start date in UTC
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Discount end date in UTC
    /// </summary>
    public DateTime EndDate { get; set; }
}
