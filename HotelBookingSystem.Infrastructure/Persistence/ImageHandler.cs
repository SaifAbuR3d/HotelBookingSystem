using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.Exceptions;
using Microsoft.AspNetCore.Http;

namespace HotelBookingSystem.Infrastructure.Persistence;

public class ImageHandler : IImageHandler
{
    private static readonly string[] allowedExtensions = [".jpg", ".jpeg", ".png"];
    public async Task<string> UploadImage(IFormFile imageData, string directory, bool thumbnail = false)
    {
        if (imageData.Length <= 0)
        {
            throw new BadFileException("Image is empty");
        }

        var imageExtension = Path.GetExtension(imageData.FileName);

        if (!allowedExtensions.Contains(imageExtension.ToLower()))
        {
            throw new BadFileException("Invalid image type");
        }

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var name = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()); // random name

        if (thumbnail)
        {
            name = "thumbnail";
        }

        var imageFullPath = Path.Combine(directory, name + imageExtension);

        if (File.Exists(imageFullPath))
        {
            File.Delete(imageFullPath);
        }

        using var stream = new FileStream(imageFullPath, FileMode.Create);
        await imageData.CopyToAsync(stream);

        return imageFullPath
               .Substring(imageFullPath.IndexOf("images"))
               .Replace("\\", "/"); // Replace backslashes with forward slashes for URL compatibility
    }
}
