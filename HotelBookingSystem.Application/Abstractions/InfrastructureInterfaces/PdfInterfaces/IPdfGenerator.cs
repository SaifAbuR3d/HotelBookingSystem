using HotelBookingSystem.Application.DTOs.Booking.OutputModel;

namespace HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.PdfInterfaces;

public interface IPdfGenerator
{
    public byte[] GeneratePdf(Invoice invoice);
}
