namespace HotelBookingSystem.Application.Exceptions;

public class NoPriceException : ServerErrorException
{
    public NoPriceException(Guid roomId) :
        base($"Error while checking Room Price, roomId: {roomId} ")
    {
    }

    public NoPriceException() :
        base("Error while checking Room Price")
    {
    }
}
