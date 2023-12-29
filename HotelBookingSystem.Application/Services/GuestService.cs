using AutoMapper;
using HotelBookingSystem.Application.DTOs.Hotel;
using HotelBookingSystem.Application.ServiceInterfaces;
using HotelBookingSystem.Domain.Abstractions.Repositories;

namespace HotelBookingSystem.Application.Services;

public class GuestService : IGuestService
{
    private readonly IGuestRepository _guestRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;


    public GuestService(IGuestRepository guestRepository,     
      IBookingRepository bookingRepository,
      IHotelRepository hotelRepository, 
      IMapper mapper)
    {
        _guestRepository = guestRepository;
        _bookingRepository = bookingRepository;
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

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
    /// </summary>
    /// <returns>An asynchronous task representing the operation, returning a collection of unique recently visited hotels.</returns>

    public async Task<IEnumerable<RecentlyVisitedHotelOutputModel>> GetRecentlyVisitedHotelsAsync(Guid guestId, int count = 5)
    {
        var recentBookings = await _guestRepository.GetRecentBookingsInDifferentHotelsAsync(guestId, count);

        return _mapper.Map<IEnumerable<RecentlyVisitedHotelOutputModel>>(recentBookings);
    }
}
