namespace HotelBookingSystem.Domain.Models;

public class Discount : Entity
{
    public decimal Percentage { get; private set; }
    public DateTime StartDate { get; private set; } // all dates/times are in UTC
    public DateTime EndDate { get; private set; }
    public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow < EndDate;
    public decimal OriginalPrice => Room.Price;
    public decimal DiscountedPrice => Math.Round(OriginalPrice - (OriginalPrice * Percentage / 100), 2);
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = default!;

    public Discount()
    {
    }
    public Discount(Room room, decimal percentage, DateTime startDate, DateTime endDate)
    {
        ValidPercentage(percentage);
        ValidDates(startDate, endDate);

        Room = room;
        Percentage = Math.Round(percentage, 2);
        StartDate = startDate;
        EndDate = endDate;

        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    public Discount(Room room, decimal originalPrice, decimal discountedPrice,
        DateTime startDate, DateTime endDate)
    {
        ValidPrices(originalPrice, discountedPrice);
        ValidDates(startDate, endDate);

        var percentage = CalculateDiscountPercentage(originalPrice, discountedPrice);
        ValidPercentage(percentage);

        Room = room;
        Percentage = percentage; 
        StartDate = startDate;
        EndDate = endDate;

        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    private static void ValidPrices(decimal originalPrice, decimal discountedPrice)
    {
        if (originalPrice <= 0 || discountedPrice <= 0)
        {
            throw new ArgumentException("Prices must be greater than 0");
        }
        if (discountedPrice >= originalPrice)
        {
            throw new ArgumentException("Discounted price must be less than original price");
        }
    }

    private static decimal CalculateDiscountPercentage(decimal originalPrice, decimal discountedPrice)
    {
        return Math.Round((originalPrice - discountedPrice) / originalPrice * 100, 2);
    }

    private static void ValidPercentage(decimal percentage)
    {
        if (percentage <= 0 || percentage > 100)
        {
            throw new ArgumentException("Percentage must be between 1 and 100");
        }
    }

    private static void ValidDates(DateTime startDate, DateTime endDate)
    {
        if (startDate < DateTime.UtcNow || endDate < DateTime.UtcNow)
        {
            throw new ArgumentException("Start and End dates must be in the future");
        }

        if (startDate > endDate)
        {
            throw new ArgumentException("Start date must be before end date");
        }
    }


}
