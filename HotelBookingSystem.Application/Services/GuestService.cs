using AutoMapper;
using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using HotelBookingSystem.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Services;

public class GuestService(IGuestRepository guestRepository,
                          IBookingRepository bookingRepository,
                          IHotelRepository hotelRepository,
                          IMapper mapper, 
                          ILogger<GuestService> logger) : IGuestService
{
    private readonly IGuestRepository _guestRepository = guestRepository;
    private readonly IBookingRepository _bookingRepository = bookingRepository;
    private readonly IHotelRepository _hotelRepository = hotelRepository;

    private readonly IMapper _mapper = mapper;

    private readonly ILogger<GuestService> _logger = logger;

    /// <summary>
    /// Retrieves a collection of unique recently visited hotels for a guest, presenting essential details.
    /// </summary>
    /// <param name="guestId">The unique identifier of the guest for whom recently visited hotels are to be retrieved.</param>
    /// <param name="count">The maximum number of unique recently visited hotels to retrieve. Default is 5.</param>
    /// <returns>An asynchronous task representing the operation, returning a collection of unique recently visited hotels.</returns>
    /// <remarks>
    /// <para>
    /// This method asynchronously retrieves recent bookings associated with the specified guest, including information about the booked rooms and their respective hotels.
    /// It then maps the relevant details to a simplified hotels model, <see cref="RecentlyVisitedHotelOutputModel"/>, for a cleaner representation of the data.
    /// The resulting collection provides essential information about the unique recently visited hotels, such as hotel name, city name, star rating, and price.
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
    /// <returns>An asynchronous task representing the operation, returning a collection of unique recently visited hotels.</returns>

    public async Task<IEnumerable<RecentlyVisitedHotelOutputModel>> GetRecentlyVisitedHotelsAsync(Guid guestId, int count = 5)
    {
        _logger.LogDebug("Validating {countRecentlyVisited}", count);
        if (count <= 0 || count > 100)
        {
            throw new BadRequestException($"invalid parameter: {count}. Number of hotels must be between 1 and 100");
        }

        _logger.LogInformation("GetRecentlyVisitedHotelsAsync started for guest with ID: {GuestId}, count: {recentlyVisitedHotelsCount}", guestId, count);

        _logger.LogDebug("Retrieving recent bookings for guest with ID: {GuestId} from the repository", guestId);
        var recentBookings = await _guestRepository.GetRecentBookingsInDifferentHotelsAsync(guestId, count);

        _logger.LogDebug("Mapping the retrieved Booking entities to RecentlyVisitedHotelOutputModel"); 
        var mapped = _mapper.Map<IEnumerable<RecentlyVisitedHotelOutputModel>>(recentBookings);

        return mapped; 
    }
}
