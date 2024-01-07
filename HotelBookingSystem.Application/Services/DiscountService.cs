using AutoMapper;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Discount;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Domain.Models;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Services;

public class DiscountService(IRoomRepository roomRepository,
                             IDiscountRepository discountRepository,
                             IMapper mapper, 
                             ILogger<DiscountService> logger) : IDiscountService
{
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly IDiscountRepository _discountRepository = discountRepository;

    private readonly IMapper _mapper = mapper;

    private readonly ILogger<DiscountService> _logger = logger;
    public async Task<DiscountOutputModel> AddDiscountAsync(Guid roomId, CreateDiscountCommand command)
    {
        _logger.LogInformation("AddDiscountAsync started for room with id: {RoomId}, request: {@Request}", roomId, command);

        _logger.LogDebug("validating the request DiscountedPrice and Percentage");
        if (command.DiscountedPrice == null && command.Percentage == null)
        {
            throw new BadRequestException("Either a discounted price or a discount percentage must be provided");
        }

        _logger.LogDebug("getting the room with id: {RoomId} from repository", roomId);
        var room = await _roomRepository.GetRoomAsync(roomId) ?? throw new NotFoundException(nameof(Room), roomId);

        // the command contains either a discounted price or a discount percentage,
        // if both are provided, ignore the percentage
        var originalPrice = room.Price;

        _logger.LogInformation("creating the discount entity for room with id: {RoomId}", roomId);
        Discount discount = default!;

        if (command.DiscountedPrice != null)
        {
            discount = new Discount(room, originalPrice, (decimal)command.DiscountedPrice, command.StartDate, command.EndDate);
        }
        else if (command.Percentage != null)
        {
            discount = new Discount(room, (decimal)command.Percentage, command.StartDate, command.EndDate);
        }

        _logger.LogInformation("created discount entity: {@Discount}", discount);

        _logger.LogDebug("adding the discount to the room with id: {RoomId}", roomId);
        room.Discounts.Add(discount);
        discount.RoomId = room.Id;

        _logger.LogDebug("Saving the created discount to the database");
        await _roomRepository.SaveChangesAsync();

        _logger.LogDebug("mapping the Discount entity to DiscountOutputModel");
        var mapped = _mapper.Map<DiscountOutputModel>(discount);

        mapped.OriginalPrice = discount.OriginalPrice;
        mapped.DiscountedPrice = discount.DiscountedPrice;

        _logger.LogInformation("AddDiscountAsync for room with id: {RoomId} completed successfully, created discount: {@Discount}",
            roomId, discount);
        return mapped;
    }

    public async Task<DiscountOutputModel?> GetDiscountAsync(Guid roomId, Guid discountId)
    {
        _logger.LogInformation("GetDiscountAsync started for Room ID: {RoomId}, Discount ID: {DiscountId}", roomId, discountId);

        _logger.LogDebug("getting the discount with id: {DiscountId} from repository", discountId);
        var discount = await _discountRepository.GetDiscountAsync(roomId, discountId) ?? throw new NotFoundException(nameof(Discount), discountId);

        _logger.LogDebug("Mapping the Discount entity to DiscountOutputModel");
        var mapped = _mapper.Map<DiscountOutputModel>(discount);

        mapped.OriginalPrice = discount.OriginalPrice;
        mapped.DiscountedPrice = discount.DiscountedPrice;

        _logger.LogInformation("GetDiscountAsync completed successfully for Room ID: {RoomId}, Discount ID: {DiscountId}", roomId, discountId);
        return mapped;

    }

    public async Task<bool> DeleteDiscountAsync(Guid roomId, Guid discountId)
    {
        _logger.LogInformation("DeleteDiscountAsync started for Room ID: {RoomId}, Discount ID: {DiscountId}", roomId, discountId);

        _logger.LogDebug("deleting the discount with id: {DiscountId} from repository (if it exists)", discountId); 
        var deleted = await _discountRepository.DeleteDiscountAsync(roomId, discountId);
        if (deleted)
        {
            _logger.LogDebug("Saving the changes to the database");
            await _discountRepository.SaveChangesAsync();
        }
        else
        {
            _logger.LogWarning("the discount with id: {DiscountId} was not found in the repository", discountId);
        }

        _logger.LogInformation("DeleteDiscountAsync completed successfully for Room ID: {RoomId}, Discount ID: {DiscountId}", roomId, discountId);
        return deleted;
    }

    public async Task<IEnumerable<FeaturedDealOutputModel>> GetFeaturedDealsAsync(int deals = 5)
    {
        _logger.LogInformation("GetFeaturedDealsAsync started with deals: {Deals}", deals);

        _logger.LogDebug("Validating number of requested deals");
        if (deals <= 0 || deals > 20)
        {
            throw new BadRequestException($"invalid number of deals: {deals}, The number of deals must be between 1 and 20");
        }

        _logger.LogDebug("getting deals (Room) entities from the repository");
        var rooms = await _roomRepository.GetRoomsWithHighestDiscounts(deals);

        if(!rooms.Any())
        {
            _logger.LogWarning("No available deals"); 
        }

        _logger.LogDebug("Mapping Room entities to FeaturedDealOutputModel"); 
        var featuredDeals = _mapper.Map<IEnumerable<FeaturedDealOutputModel>>(rooms);

        _logger.LogInformation("GetFeaturedDealsAsync with deals: {Deals} completed successfully", deals);
        return featuredDeals;

    }


}
