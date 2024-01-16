namespace HotelBookingSystem.Domain.Models;

public class Guest : Entity
{
    public Guest(string firstName, string lastName)
    {
        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;

        FirstName = firstName;
        LastName = lastName;
    }
    public Guest()
    {
        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
