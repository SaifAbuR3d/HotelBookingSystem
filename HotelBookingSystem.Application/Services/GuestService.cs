using AutoMapper;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Application.Identity;
using HotelBookingSystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace HotelBookingSystem.Application.Services;

public class GuestService(IGuestRepository guestRepository,
                          IBookingRepository bookingRepository,
                          IHotelRepository hotelRepository,
                          IMapper mapper,
                          IHttpContextAccessor httpContextAccessor,
                          ILogger<GuestService> logger) : IGuestService
{
    private readonly IGuestRepository _guestRepository = guestRepository;
    private readonly IBookingRepository _bookingRepository = bookingRepository;
    private readonly IHotelRepository _hotelRepository = hotelRepository;

    private readonly IMapper _mapper = mapper;

    private readonly ILogger<GuestService> _logger = logger;

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ;


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

        _logger.LogDebug("Getting the user id from HttpContext"); 
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? throw new UnauthenticatedException();

        _logger.LogDebug("Getting the user role from HttpContext");
        var role = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role)
            ?? throw new UnauthenticatedException();

        _logger.LogDebug("Checking if the user is a guest");
        if (role != UserRoles.Guest)
        {
            throw new BadRequestException($"Invalid role: {role} at GetRecentlyVisitedHotelsAsync, user should be a {UserRoles.Guest}");
        }

        _logger.LogDebug("Getting the guest id from the repository");
        var guestId = await _guestRepository.GetGuestIdByUserIdAsync(userId) 
            ?? throw new NotFoundException(nameof(Guest), userId);

        _logger.LogDebug("Calling GetRecentlyVisitedHotelsAsync with guestId: {guestId}, count: {count}", guestId, count);
        var result = await GetRecentlyVisitedHotelsAsync(guestId, count);

        _logger.LogInformation("GetRecentlyVisitedHotelsAsync finished for current user, count: {recentlyVisitedHotelsCount}", count);
        return result;
       
    }
}
