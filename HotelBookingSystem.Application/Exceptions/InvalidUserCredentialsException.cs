namespace HotelBookingSystem.Application.Exceptions;

public class InvalidUserCredentialsException : Exception
{
    public InvalidUserCredentialsException()
        : base("Invalid credentials.")
    {
    }
}
