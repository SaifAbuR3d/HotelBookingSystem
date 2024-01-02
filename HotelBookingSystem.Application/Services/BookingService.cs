using AutoMapper;
using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Booking;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public BookingService(IRoomRepository roomRepository,
                              IGuestRepository guestRepository,
                              IBookingRepository bookingRepository,
                              IMapper mapper)
        {
            _roomRepository = roomRepository;
            _guestRepository = guestRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<BookingConfirmationOutputModel?> GetBookingAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetBookingAsync(bookingId) ?? throw new NotFoundException(nameof(Booking), bookingId);

            var mapped = _mapper.Map<BookingConfirmationOutputModel>(booking);

            return mapped;
        }

        public async Task<BookingConfirmationOutputModel> CreateBookingAsync(CreateBookingCommand request)
        {
            (Guid guestId, Guid roomId, DateOnly checkInDate, DateOnly checkOutDate, int NumberOfAdults, int NumberOfChildren) =
             (request.GuestId, request.RoomId, request.CheckInDate, request.CheckOutDate, request.NumberOfAdults, request.NumberOfChildren);

            var room = await _roomRepository.GetRoomAsync(roomId) ?? throw new NotFoundException(nameof(Room), roomId);
            var guest = await _guestRepository.GetGuestAsync(guestId) ?? throw new NotFoundException(nameof(Guest), guestId);

            bool isAvailable = await _roomRepository.IsAvailableAsync(roomId, checkInDate, checkOutDate);

            if (NumberOfAdults > room.AdultsCapacity || NumberOfChildren > room.ChildrenCapacity)
            {
                throw new InvalidNumberOfGuestsException(roomId, NumberOfAdults, NumberOfChildren);
            }

            if (!isAvailable)
            {
                throw new UnavailableRoomException(roomId, checkInDate, checkOutDate);
            }
            
            var booking = _mapper.Map<Booking>(request);

            booking.Id = Guid.NewGuid();
            booking.CreationDate = DateTime.UtcNow;
            booking.LastModified = DateTime.UtcNow;
            booking.Room = room;
            booking.Guest = guest;
            booking.Price = room.Price;

            await _bookingRepository.AddBookingAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            return _mapper.Map<BookingConfirmationOutputModel>(booking);
        }

        public async Task<bool> DeleteBookingAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetBookingAsync(bookingId) ?? throw new NotFoundException(nameof(Booking), bookingId);

            await _bookingRepository.DeleteBookingAsync(bookingId);

            await _bookingRepository.SaveChangesAsync();

            return true;
        }

    }
}
