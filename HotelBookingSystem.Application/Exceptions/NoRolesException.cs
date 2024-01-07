namespace HotelBookingSystem.Application.Exceptions;

public class NoRolesException : Exception
{
    public NoRolesException(string userId) : base($"No Roles For the user with Id {userId}")
    {
    }
}
