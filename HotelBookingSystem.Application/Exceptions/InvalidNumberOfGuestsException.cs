namespace HotelBookingSystem.Application.Exceptions;

public class InvalidNumberOfGuestsException : CustomException
{
    public InvalidNumberOfGuestsException(int numberOfAdults, int numberOfChildren)
            : base($"requested Rooms cannot accommodate {numberOfAdults} Adults and {numberOfChildren} Children.")
    { }

    public InvalidNumberOfGuestsException(string message) : base(message)
    { }
}
