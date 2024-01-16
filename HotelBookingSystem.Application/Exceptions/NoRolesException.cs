namespace HotelBookingSystem.Application.Exceptions;

public class NoRolesException : ServerErrorException
{
    public NoRolesException(string userId) : base($"No Roles For the user with Id {userId}")
    {
    }
}
