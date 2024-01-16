namespace HotelBookingSystem.Application.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string userId, string role) :
        base($"The user with Id {userId} has no role {role}")
    {
    }

    public UnauthorizedException(string userId, Guid guestId) :
        base($"The user with Id {userId} has no access to guest with Id {guestId}")
    {
    }
    public UnauthorizedException(string userId, string entityName, object entityId) :
        base($"The user with Id {userId} has no access to Entity '{entityName}' ({entityId})")
    {
    }
}
