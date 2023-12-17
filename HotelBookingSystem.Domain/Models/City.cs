namespace HotelBookingSystem.Domain.Models;

public class City : Entity
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string PostOffice { get; set; }

    public ICollection<Hotel> Hotels { get; set; }
    
}
