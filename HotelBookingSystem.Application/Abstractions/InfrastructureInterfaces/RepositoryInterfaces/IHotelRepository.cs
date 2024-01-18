using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using HotelBookingSystem.Application.DTOs.Hotel.Query;
using HotelBookingSystem.Domain.Models;
namespace HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;

public interface IHotelRepository
{
    Task<Hotel> AddHotelAsync(Hotel hotel); // 
    Task<bool> HotelExistsAsync(Guid id);
    Task<bool> DeleteHotelAsync(Guid id);
    Task<(IEnumerable<Hotel>, PaginationMetadata)> GetAllHotelsAsync(GetHotelsQueryParameters request);
    Task<Hotel?> GetHotelAsync(Guid id);
    Task<bool> SaveChangesAsync();
    Task<Hotel?> GetHotelByNameAsync(string Name);
    Task<HotelImage> AddHotelImageAsync(Hotel hotel, HotelImage hotelImage);
    Task<(IEnumerable<Hotel>, PaginationMetadata)> SearchAndFilterHotelsAsync(HotelSearchAndFilterParameters request);
}
