using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.EmailInterfaces;
using HotelBookingSystem.Application.DTOs.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Cryptography;

namespace HotelBookingSystem.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings) 
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mail)
        {
            var email = GetMimeMessage(mail);

            using var smtp = new SmtpClient();
            smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);
            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }

        private MimeMessage GetMimeMessage(MailRequest mail)
        {
            var email = new MimeMessage();

            email.Sender = MailboxAddress.Parse(_emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(mail.ToEmail));
            email.Subject = mail.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = AsHtml(mail.Body); 
            builder.Attachments.Add("invoice.pdf", mail.Attachment, new ContentType("application", "pdf"));

            email.Body = builder.ToMessageBody();
            return email;
        }

        private static string AsHtml(string body)
        {
            return $"<h1>{body}</h1>";
        }
    }
}
