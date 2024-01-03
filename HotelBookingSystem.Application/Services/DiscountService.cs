using AutoMapper;
using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Discount;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Services;

public class DiscountService(IRoomRepository roomRepository, IDiscountRepository discountRepository,  IMapper mapper) : IDiscountService
{
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly IDiscountRepository _discountRepository = discountRepository;
    private readonly IMapper _mapper = mapper;
    public async Task<DiscountOutputModel> AddDiscountAsync(Guid roomId, CreateDiscountCommand command)
    {
        if (command.DiscountedPrice == null && command.Percentage == null)
        {
            throw new BadRequestException("Either a discounted price or a discount percentage must be provided");
        }

        var room = await _roomRepository.GetRoomAsync(roomId) ?? throw new NotFoundException(nameof(Room), roomId);

        // the command contains either a discounted price or a discount percentage,
        // if both are provided, ignore the percentage
        var originalPrice = room.Price;

        Discount discount = default!; 

        if (command.DiscountedPrice != null)
        {
            discount = new Discount(room, originalPrice, (decimal)command.DiscountedPrice, command.StartDate, command.EndDate);
        }
        else if (command.Percentage != null)
        {
            discount = new Discount(room, (decimal)command.Percentage, command.StartDate, command.EndDate);
        }

        room.Discounts.Add(discount);
        discount.RoomId = room.Id;

        await _roomRepository.SaveChangesAsync();

        var mapped = _mapper.Map<DiscountOutputModel>(discount);

        mapped.OriginalPrice = discount.OriginalPrice; 
        mapped.DiscountedPrice = discount.DiscountedPrice;

        return mapped; 
    }

    public async Task<DiscountOutputModel?> GetDiscountAsync(Guid roomId, Guid discountId)
    {
        var discount = await _discountRepository.GetDiscountAsync(roomId, discountId) ?? throw new NotFoundException(nameof(Discount), discountId);
        
        var mapped = _mapper.Map<DiscountOutputModel>(discount);

        mapped.OriginalPrice = discount.OriginalPrice;
        mapped.DiscountedPrice = discount.DiscountedPrice;

        return mapped;

    }

    public async Task<bool> DeleteDiscountAsync(Guid roomId, Guid discountId)
    {
        var deleted = await _discountRepository.DeleteDiscountAsync(roomId, discountId);
        if (deleted)
        {
            await _discountRepository.SaveChangesAsync();
        }
        return deleted;
    }

}
