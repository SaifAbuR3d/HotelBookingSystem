using HotelBookingSystem.Application.DTOs.Booking.Command;
using HotelBookingSystem.Application.DTOs.Booking.OutputModel;

namespace HotelBookingSystem.Application.Abstractions.ServiceInterfaces;

public interface IBookingService
{
    Task<BookingOutputModel> CreateBookingAsync(CreateBookingCommand request);
    Task<BookingOutputModel?> GetBookingAsync(Guid bookingId);
    Task<Invoice?> GetInvoiceAsync(Guid bookingId);
    Task<bool> DeleteBookingAsync(Guid bookingId);
    Task<byte[]> GetInvoicePdfByBookingIdAsync(Guid bookingId);
}