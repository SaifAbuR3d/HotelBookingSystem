namespace HotelBookingSystem.Application.DTOs.Email;

public class MailRequest
{
    public string ToEmail { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
    public byte[] Attachment { get; set; } = default!;
}
