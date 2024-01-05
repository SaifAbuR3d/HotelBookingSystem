namespace HotelBookingSystem.Application.Exceptions;

public class InvalidNumberOfGuestsException : CustomException
{
    public InvalidNumberOfGuestsException(Guid roomId, int numberOfAdults, int numberOfChildren)
            : base($"Room with id: '{roomId}' cannot accommodate {numberOfAdults} Adults and {numberOfChildren} Children.")
    { }
}
