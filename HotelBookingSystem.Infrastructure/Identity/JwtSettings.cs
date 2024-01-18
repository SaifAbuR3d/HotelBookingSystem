namespace HotelBookingSystem.Infrastructure.Identity;

public class JwtSettings
{
    public string Key { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public int TokenExpirationInHours { get; set; }
}
