namespace HotelBookingSystem.Application.Exceptions;

public class BadRequestException : CustomException
{
    public BadRequestException(string message) : base(message)
    { }
}
