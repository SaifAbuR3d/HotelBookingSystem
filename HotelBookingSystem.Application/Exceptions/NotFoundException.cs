namespace HotelBookingSystem.Application.Exceptions;

public class NotFoundException : CustomException
{
    public NotFoundException(string name, object key)
        : base($"Entity '{name}' ({key}) was not found.")
    { }

    public NotFoundException(string name)
        : base($"Entity '{name}' was not found.")
    { }
}
