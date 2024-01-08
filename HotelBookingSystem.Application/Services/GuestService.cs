using AutoMapper;
using HotelBookingSystem.Application.Abstractions;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Application.Identity;
using HotelBookingSystem.Domain.Models;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Services;

public class GuestService(IGuestRepository guestRepository,
                          IBookingRepository bookingRepository,
                          IHotelRepository hotelRepository,
                          IMapper mapper,
                          ICurrentUser currentUser,
                          ILogger<GuestService> logger) : IGuestService
{
    private readonly IGuestRepository _guestRepository = guestRepository;
    private readonly IBookingRepository _bookingRepository = bookingRepository;
    private readonly IHotelRepository _hotelRepository = hotelRepository;

    private readonly IMapper _mapper = mapper;

    private readonly ILogger<GuestService> _logger = logger;

    private readonly ICurrentUser _currentUser = currentUser;

    /// <summary>
    /// Retrieves a collection of unique recently visited hotels for a guest, presenting essential details.
    /// </summary>
    /// <param name="guestId">
    /// The unique identifier of the guest for whom recently visited hotels are to be retrieved.
    /// </param>
    /// <param name="count">The maximum number of unique recently visited hotels to retrieve. Default is 5.</param>
    /// <remarks>
    /// <para>
    /// This method asynchronously retrieves recent bookings associated with the specified guest,
    /// including information about the booked rooms and their respective hotels.
    /// It then maps the relevant details to a simplified hotels model, <see cref="RecentlyVisitedHotelOutputModel"/>,
    /// for a cleaner representation of the data.
    /// The resulting collection provides essential information about the unique recently visited hotels,
    /// such as hotel name, city name, star rating, and price.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var guestId = Guid.Parse("your_guest_id_here");
    /// var recentHotels = await service.GetRecentlyVisitedHotels(guestId, 5);
    /// // Process the recentHotels collection as needed.
    /// </code>
    /// </example>
    /// <seealso cref="RecentlyVisitedHotelOutputModel"/>
    /// <returns>
    /// An asynchronous task representing the operation, returning a collection of unique recently visited hotels.
    /// </returns>

    public async Task<IEnumerable<RecentlyVisitedHotelOutputModel>> GetRecentlyVisitedHotelsAsync(Guid guestId, int count = 5)
    {
        _logger.LogInformation("GetRecentlyVisitedHotelsAsync started for guest with ID: {GuestId}, count: {recentlyVisitedHotelsCount}", guestId, count);

        _logger.LogDebug("Validating {countRecentlyVisited}", count);
        if (count <= 0 || count > 100)
        {
            throw new BadRequestException($"invalid parameter: {count}. Number of hotels must be between 1 and 100");
        }

        _logger.LogDebug("Checking if guest with id: {guestId} exists", guestId);
        bool guestExists = await _guestRepository.GuestExistsAsync(guestId);

        if(!guestExists)
        {
            throw new NotFoundException(nameof(Guest), guestId);
        }

        _logger.LogDebug("Retrieving recent bookings for guest with ID: {GuestId} from the repository", guestId);
        var recentBookings = await _guestRepository.GetRecentBookingsInDifferentHotelsAsync(guestId, count);

        _logger.LogDebug("Mapping the retrieved Booking entities to RecentlyVisitedHotelOutputModel"); 
        var mapped = _mapper.Map<IEnumerable<RecentlyVisitedHotelOutputModel>>(recentBookings);

        return mapped; 
    }

    /// <summary>
    /// Retrieves a collection of unique recently visited hotels for a guest (current logged in guest),
    /// presenting essential details.
    /// </summary>
    /// <param name="count">
    /// The maximum number of unique recently visited hotels to retrieve. Default is 5.
    /// </param>
    /// <remarks>
    /// <para>
    /// This method asynchronously retrieves recent bookings associated with the specified guest,
    /// including information about the booked rooms and their respective hotels.
    /// It then maps the relevant details to a simplified hotels model, <see cref="RecentlyVisitedHotelOutputModel"/>,
    /// for a cleaner representation of the data.
    /// The resulting collection provides essential information about the unique recently visited hotels,
    /// such as hotel name, city name, star rating, and price.
    /// </para>
    /// <para>
    /// This method is intended to be used by the current logged in guest, and therefore does not require a guestId parameter.
    /// </para>
    /// <para>
    /// This method internally depends on the <see cref="GetRecentlyVisitedHotelsAsync(Guid, int)"/>
    /// </para>
    /// </remarks>
    /// <seealso cref="RecentlyVisitedHotelOutputModel"/>
    /// <returns>
    /// An asynchronous task representing the operation, returning a collection of unique recently visited hotels.
    /// </returns>
    public async Task<IEnumerable<RecentlyVisitedHotelOutputModel>> GetRecentlyVisitedHotelsAsync(int count = 5)
    {
        _logger.LogInformation("GetRecentlyVisitedHotelsAsync started for current user, count: {recentlyVisitedHotelsCount}", count);

        var (guest, _) = await GetGuestFromCurrentUser();

        _logger.LogDebug("Calling GetRecentlyVisitedHotelsAsync with guestId: {guestId}, count: {count}", guest.Id, count);
        var result = await GetRecentlyVisitedHotelsAsync(guest.Id, count);

        _logger.LogInformation("GetRecentlyVisitedHotelsAsync completed successfully for current user, count: {recentlyVisitedHotelsCount}, with guestId: {@guestId}", count, guest.Id);
        return result;
       
    }

    private async Task<(Guest, string)> GetGuestFromCurrentUser()
    {
        _logger.LogDebug("Getting the user id from CurrentUser");
        var userId = _currentUser.Id;

        _logger.LogDebug("Getting the guest from the repository");
        var guest = await _guestRepository.GetGuestByUserIdAsync(userId)
            ?? throw new NotFoundException(nameof(Guest), userId);

        return (guest, userId);
    }
}
