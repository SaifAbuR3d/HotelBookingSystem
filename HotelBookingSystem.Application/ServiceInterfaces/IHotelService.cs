using HotelBookingSystem.Application.DTOs.Hotel;

namespace HotelBookingSystem.Application.ServiceInterfaces;

public interface IHotelService
{
    Task<HotelOutputModel> CreateHotelAsync(CreateHotelCommand command);
    Task<bool> DeleteHotelAsync(Guid id);
    Task<HotelOutputModel?> GetHotelAsync(Guid id);
    Task<IEnumerable<HotelOutputModel>> GetAllHotelsAsync();
    Task<bool> UpdateHotelAsync(Guid id, UpdateHotelCommand request);
}
