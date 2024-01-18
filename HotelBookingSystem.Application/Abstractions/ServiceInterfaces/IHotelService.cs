using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Hotel.Command;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using HotelBookingSystem.Application.DTOs.Hotel.Query;
using HotelBookingSystem.Application.DTOs.Review;
using Microsoft.AspNetCore.Http;

namespace HotelBookingSystem.Application.Abstractions.ServiceInterfaces;

public interface IHotelService
{
    Task<HotelOutputModel> CreateHotelAsync(CreateHotelCommand command);
    Task<bool> DeleteHotelAsync(Guid id);
    Task<HotelOutputModel?> GetHotelAsync(Guid id);
    Task<(IEnumerable<HotelOutputModel>, PaginationMetadata)> GetAllHotelsAsync(GetHotelsQueryParameters request);
    Task<bool> UpdateHotelAsync(Guid id, UpdateHotelCommand request);
    Task<bool> UploadImageAsync(Guid hotelId, IFormFile file, string basePath, string? alternateText, bool? thumbnail = false);
    Task<(IEnumerable<HotelSearchResultOutputModel>, PaginationMetadata)> SearchAndFilterHotelsAsync(HotelSearchAndFilterParameters request);
}
