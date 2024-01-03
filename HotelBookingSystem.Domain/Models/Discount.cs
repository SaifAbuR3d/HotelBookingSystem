namespace HotelBookingSystem.Domain.Models;

public class Discount : Entity
{
    public double Percentage { get; private set; }
    public DateTime StartDate { get; private set; } // all dates/times are in UTC
    public DateTime EndDate { get; private set; }
    public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = default!;

    public Discount()
    {
    }
    public Discount(Room room, double percentage, DateTime startDate, DateTime endDate)
    {
        Room = room;
        Percentage = Math.Round(percentage, 2);
        StartDate = startDate;
        EndDate = endDate;

        ValidPercentage(percentage);
        ValidDates(startDate, endDate);
    }

    public Discount(Room room, decimal originalPrice, decimal discountedPrice,
        DateTime startDate, DateTime endDate)
    {
        Room = room;
        Percentage = CalculateDiscountPercentage(originalPrice, discountedPrice);

        ValidDates(startDate, endDate);
        ValidPercentage(Percentage);
    }

    private static double CalculateDiscountPercentage(decimal originalPrice, decimal discountedPrice)
    {
        return (double)Math.Round((originalPrice - discountedPrice) / originalPrice * 100, 2);
    }

    private static void ValidPercentage(double percentage)
    {
        if (percentage < 0 || percentage > 100)
        {
            throw new ArgumentException("Percentage must be between 0 and 100");
        }
    }

    private static void ValidDates(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException("Start date must be before end date");
        }

        if (startDate < DateTime.UtcNow || endDate < DateTime.UtcNow)
        {
            throw new ArgumentException("End date must be in the future");
        }
    }


}
