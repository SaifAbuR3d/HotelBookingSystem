namespace HotelBookingSystem.Domain.Models;

public abstract class Entity
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
}
