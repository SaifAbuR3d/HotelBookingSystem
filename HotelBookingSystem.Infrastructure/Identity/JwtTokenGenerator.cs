using HotelBookingSystem.Application.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelBookingSystem.Infrastructure.Identity;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly string? key; 
    private readonly string? issuer;
    public JwtTokenGenerator(IConfiguration configuration)
    {
        key = configuration.GetSection("JwtConfig").GetSection("Key").Value;
        issuer = configuration.GetSection("JwtConfig").GetSection("Issuer").Value;
    }
    public string GenerateToken(ApplicationUser user, IList<string> roles)
    {
        if (key is null || issuer is null)
        {
            throw new Exception("JwtConfig section is missing from appsettings.json");
        }

        var claims = getClaims(user, roles);

        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

        var token = new JwtSecurityToken(
            issuer: issuer,
            expires: DateTime.UtcNow.AddHours(12),
            claims: claims,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

        var encryptedToken = new JwtSecurityTokenHandler()
           .WriteToken(token);

        return encryptedToken;
    }

    private static List<Claim> getClaims(ApplicationUser user, IList<string> roles)
    {
        var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Name, user.Email)
                };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}
