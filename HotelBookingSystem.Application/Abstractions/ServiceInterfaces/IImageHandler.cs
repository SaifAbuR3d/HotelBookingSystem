using Microsoft.AspNetCore.Http;

namespace HotelBookingSystem.Application.Abstractions.ServiceInterfaces
{
    public interface IImageHandler
    {
        Task<string> UploadImage(IFormFile file, string directory, bool thumbnail = false);
    }
}