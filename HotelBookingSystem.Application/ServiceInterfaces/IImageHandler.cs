using Microsoft.AspNetCore.Http;

namespace HotelBookingSystem.Application.ServiceInterfaces
{
    public interface IImageHandler
    {
        Task<string> UploadImage(IFormFile file, string directory, bool thumbnail = false);
    }
}