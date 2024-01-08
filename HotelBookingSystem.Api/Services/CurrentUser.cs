using HotelBookingSystem.Application.Abstractions;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Application.Identity;
using System.Security.Claims;

namespace HotelBookingSystem.Api.Services;

/// <summary>
/// Get information about the current authenticated user
/// </summary>
/// <param name="httpContextAccessor"></param>
public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    /// <summary>
    /// Get the Id of the current authenticated user
    /// </summary>
    public string Id => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthenticatedException();

    /// <summary>
    /// Get the role of the current authenticated user
    /// </summary>
    public string Role => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role)
            ?? throw new UnauthenticatedException();

    /// <summary>
    /// returns true is the current authenticated user is a guest
    /// </summary>
    public bool IsGuest => Role == UserRoles.Guest;

    /// <summary>
    /// returns true is the current authenticated user is an admin
    /// </summary>
    public bool IsAdmin => Role == UserRoles.Admin;
}