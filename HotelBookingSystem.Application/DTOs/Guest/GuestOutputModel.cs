namespace HotelBookingSystem.Application.DTOs.Guest;

public class GuestOutputModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public int NumberOfBookings { get; set; }
    
}
