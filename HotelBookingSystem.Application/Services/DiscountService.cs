using AutoMapper;
using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Discount;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Services;

public class DiscountService(IRoomRepository roomRepository, IMapper mapper) : IDiscountService
{
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly IMapper _mapper = mapper;
    public async Task<DiscountOutputModel> AddDiscountAsync(CreateDiscountCommand command)
    {
        if (command.DiscountedPrice == null && command.Percentage == null)
        {
            throw new BadRequestException("Either a discounted price or a discount percentage must be provided");
        }

        var room = await _roomRepository.GetRoomAsync(command.RoomId) ?? throw new NotFoundException(nameof(Room), command.RoomId);

        // the command contains either a discounted price or a discount percentage,
        // if both are provided, ignore the percentage
        var originalPrice = room.Price;

        Discount discount = default!; 

        if (command.DiscountedPrice != null)
        {
            discount = new Discount(room, (decimal)command.DiscountedPrice, originalPrice, command.StartDate, command.EndDate);
        }
        else if (command.Percentage != null)
        {
            discount = new Discount(room, (double)command.Percentage, command.StartDate, command.EndDate);
        }

        room.Discounts.Add(discount);
        discount.RoomId = room.Id;

        await _roomRepository.SaveChangesAsync();

        return _mapper.Map<DiscountOutputModel>(discount);
    }

}
