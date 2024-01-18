using HotelBookingSystem.Application.DTOs.Email;

namespace HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.EmailInterfaces;

public interface IEmailService
{
    Task SendEmailAsync(MailRequest mail);
}
