namespace HotelBookingSystem.Application.Exceptions;

public class UnavailableRoomException : Exception
{
    public UnavailableRoomException(Guid roomId, DateOnly startDate, DateOnly endDate)
        : base($"Room with id: '{roomId}' is Unavailable from {startDate} to {endDate}.")
    { }
}
