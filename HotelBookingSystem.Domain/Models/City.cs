namespace HotelBookingSystem.Domain.Models;

public class City : Entity
{
    public string Name { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string PostOffice { get; set; } = default!;  
    public ICollection<CityImage> Images { get; set; } = new List<CityImage>();
    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>(); 
    
}
