using HotelBookingSystem.Application.Abstractions;
using HotelBookingSystem.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelBookingSystem.Infrastructure.Identity;

public class ApplicationUser : IdentityUser, IUser
{
    public ApplicationUser(string userName, string email) : base(userName: userName)
    {
        Email = email;
    }
    public Guest? Guest { get; private set; }

    public void BecomeGuest(Guest guest)
    {
        Guest = guest;
    }
}
