using AutoMapper;
using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Booking;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Domain.Models;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Services
{
    public class BookingService(IRoomRepository roomRepository,
                                IGuestRepository guestRepository,
                                IBookingRepository bookingRepository,
                                IMapper mapper,
                                ILogger<BookingService> logger) : IBookingService
    {
        private readonly IRoomRepository _roomRepository = roomRepository;
        private readonly IGuestRepository _guestRepository = guestRepository;
        private readonly IBookingRepository _bookingRepository = bookingRepository;

        private readonly ILogger<BookingService> _logger = logger;

        private readonly IMapper _mapper = mapper;

        public async Task<BookingConfirmationOutputModel?> GetBookingAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetBookingAsync(bookingId) ?? throw new NotFoundException(nameof(Booking), bookingId);

            var mapped = _mapper.Map<BookingConfirmationOutputModel>(booking);

            return mapped;
        }

        public async Task<BookingConfirmationOutputModel> CreateBookingAsync(CreateBookingCommand request)
        {
            _logger.LogInformation("CreateBookingAsync started for request: {@CreateBooking}", request);

            (Guid guestId, Guid roomId, DateOnly checkInDate, DateOnly checkOutDate, int NumberOfAdults, int NumberOfChildren) =
             (request.GuestId, request.RoomId, request.CheckInDate, request.CheckOutDate, request.NumberOfAdults, request.NumberOfChildren);

            _logger.LogDebug("Retrieving room with ID: {RoomId}", roomId);
            var room = await _roomRepository.GetRoomAsync(roomId) ?? throw new NotFoundException(nameof(Room), roomId);

            _logger.LogDebug("Retrieving guest with ID: {GuestId}", guestId);
            var guest = await _guestRepository.GetGuestAsync(guestId) ?? throw new NotFoundException(nameof(Guest), guestId);

            _logger.LogDebug("Checking room availability for room with ID: {RoomId}", roomId);
            bool isAvailable = await _roomRepository.IsAvailableAsync(roomId, checkInDate, checkOutDate);

            if (NumberOfAdults > room.AdultsCapacity || NumberOfChildren > room.ChildrenCapacity)
            {
                _logger.LogWarning("Invalid number of guests for room with ID: {RoomId}. Adults: {NumberOfAdults}, Children: {NumberOfChildren}", roomId, NumberOfAdults, NumberOfChildren);
                throw new InvalidNumberOfGuestsException(roomId, NumberOfAdults, NumberOfChildren);
            }

            if (!isAvailable)
            {
                _logger.LogWarning("Room with ID: {RoomId} is not available for the specified dates: {CheckInDate} to {CheckOutDate}", roomId, checkInDate, checkOutDate);
                throw new UnavailableRoomException(roomId, checkInDate, checkOutDate);
            }

            _logger.LogDebug("Mapping the request to a Booking entity");
            var booking = _mapper.Map<Booking>(request);

            booking.Id = Guid.NewGuid();
            booking.CreationDate = DateTime.UtcNow;
            booking.LastModified = DateTime.UtcNow;
            booking.Room = room;
            booking.Guest = guest;
            booking.Price = room.Price;

            _logger.LogInformation("Adding the booking to the repository");
            await _bookingRepository.AddBookingAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            _logger.LogDebug("Mapping the booking entity to BookingConfirmationOutputModel");
            var mapped = _mapper.Map<BookingConfirmationOutputModel>(booking);

            _logger.LogInformation("CreateBookingAsync completed successfully. Booking ID: {BookingId}", booking.Id);
            return mapped; 
        }

        public async Task<bool> DeleteBookingAsync(Guid bookingId)
        {
            _logger.LogInformation("DeleteBookingAsync started for booking ID: {BookingId}", bookingId);

            _logger.LogDebug("Retrieving booking with ID: {BookingId}", bookingId);
            var booking = await _bookingRepository.GetBookingAsync(bookingId) ?? throw new NotFoundException(nameof(Booking), bookingId);

            _logger.LogInformation("Deleting booking with ID from the Repository: {BookingId}", bookingId);
            await _bookingRepository.DeleteBookingAsync(bookingId);
            await _bookingRepository.SaveChangesAsync();

            _logger.LogInformation("DeleteBookingAsync completed successfully. Booking ID: {BookingId}", bookingId);
            return true;
        }

    }
}
