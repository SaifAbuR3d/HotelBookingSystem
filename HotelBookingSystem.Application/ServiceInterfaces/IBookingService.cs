using HotelBookingSystem.Application.DTOs.Booking;

namespace HotelBookingSystem.Application.ServiceInterfaces;

public interface IBookingService
{
    Task<BookingConfirmationOutputModel?> GetBookingAsync(Guid bookingId); 
    Task<BookingConfirmationOutputModel> CreateBookingAsync(CreateBookingCommand createBookingCommand);
    Task<bool> DeleteBookingAsync(Guid bookingId);

}