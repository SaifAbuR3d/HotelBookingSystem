using AutoMapper;
using HotelBookingSystem.Application.Abstractions;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.EmailInterfaces;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.PdfInterfaces;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Booking.Command;
using HotelBookingSystem.Application.DTOs.Booking.OutputModel;
using HotelBookingSystem.Application.DTOs.Email;
using HotelBookingSystem.Application.DTOs.Room.OutputModel;
using HotelBookingSystem.Application.Exceptions;
using HotelBookingSystem.Domain.Models;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Services;

public class BookingService(IHotelRepository hotelRepository,
                            IRoomRepository roomRepository,
                            IGuestRepository guestRepository,
                            IBookingRepository bookingRepository,
                            IMapper mapper,
                            ICurrentUser currentUser,
                            IPdfGenerator pdfService,
                            IEmailService emailService,
                            ILogger<BookingService> logger) : IBookingService
{
    private readonly IHotelRepository _hotelRepository = hotelRepository;
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly IGuestRepository _guestRepository = guestRepository;
    private readonly IBookingRepository _bookingRepository = bookingRepository;

    private readonly IMapper _mapper = mapper;

    private readonly ICurrentUser _currentUser = currentUser;

    private readonly IPdfGenerator _pdfService = pdfService;

    private readonly IEmailService _emailService = emailService;

    private readonly ILogger<BookingService> _logger = logger;



    public async Task<BookingOutputModel?> GetBookingAsync(Guid bookingId)
    {
        var booking = await _bookingRepository.GetBookingAsync(bookingId)
            ?? throw new NotFoundException(nameof(Booking), bookingId);

        var (guest, userId) = await GetGuestFromCurrentUser();

        if (booking.GuestId != guest.Id)
        {
            throw new UnauthorizedException(userId, nameof(Booking), booking.Id);
        }

        var mapped = _mapper.Map<BookingOutputModel>(booking);

        return mapped;
    }

    public async Task<Invoice?> GetInvoiceAsync(Guid bookingId)
    {
        var booking = await _bookingRepository.GetBookingAsync(bookingId)
            ?? throw new NotFoundException(nameof(Booking), bookingId);

        var (guest, userId) = await GetGuestFromCurrentUser();

        if(booking.GuestId != guest.Id)
        {
            throw new UnauthorizedException(userId, nameof(Booking), booking.Id);
        }

        var invoice = await ConvertBookingToInvoiceAsync(booking);

        return invoice;
    }

    private async Task<Invoice> ConvertBookingToInvoiceAsync(Booking booking)
    {
        _logger.LogInformation("Converting booking to invoice. Booking ID: {BookingId}", booking.Id);

        var invoice = _mapper.Map<Invoice>(booking);

        _logger.LogDebug("Converting Rooms to RoomsWithinInvoice");
        var roomsWithinInvoice = new List<RoomWithinInvoice>();
        foreach (var room in booking.Rooms)
        {
            var roomInvoice = await ConvertRoomToRoomWithinInvoice(room, booking.CheckInDate, booking.CheckOutDate);
            roomsWithinInvoice.Add(roomInvoice);
        }

        invoice.Rooms = roomsWithinInvoice;
        invoice.TotalPrice = roomsWithinInvoice.Sum(r => r.TotalRoomPrice);
        invoice.TotalPriceAfterDiscount = booking.Price;  // Same as roomsWithinInvoice.Sum(r => r.TotalRoomPriceAfterDiscount);

        _logger.LogInformation("Converting booking to invoice Completed successfully, Invoice: {invoice}", invoice);

        return invoice;
    }

    // Convert the Room to RoomWithinInvoice
    // which contains the price of the room with discounts (if there is any)
    private async Task<RoomWithinInvoice> ConvertRoomToRoomWithinInvoice(Room room, DateOnly checkInDate, DateOnly checkOutDate)
    {
        _logger.LogInformation("Starting to get RoomWithinInvoice for Room ID: {roomId}", room.Id);

        // get the price of the room with discounts (if there is any)
        _logger.LogDebug("Getting room price per night for Room ID: {roomId}", room.Id);
        var pricePerNight = await _roomRepository.GetPrice(room.Id, checkInDate, checkOutDate)
            ?? throw new NoPriceException(room.Id);

        var numberOfNights = checkOutDate.DayNumber - checkInDate.DayNumber;
        _logger.LogDebug("Calculated number of nights: {NumberOfNights} for Room ID: {RoomId}", numberOfNights, room.Id);

        var roomInInvoice = _mapper.Map<RoomWithinInvoice>(room);
        roomInInvoice.PricePerNight = room.Price;

        roomInInvoice.PricePerNightAfterDiscount = pricePerNight;
        roomInInvoice.NumberOfNights = numberOfNights;
        roomInInvoice.TotalRoomPrice = room.Price * roomInInvoice.NumberOfNights;
        roomInInvoice.TotalRoomPriceAfterDiscount = pricePerNight * roomInInvoice.NumberOfNights;

        _logger.LogInformation("Successfully created RoomWithinInvoice for Room ID: {RoomId}, {RoomWithinInvoice}",
            room.Id, roomInInvoice);
        return roomInInvoice;
    }

    private async Task CanGuestDeleteTheBooking(Booking booking)
    {
        _logger.LogDebug("Getting the guest id from CurrentUser");
        var (guest, userId) = await GetGuestFromCurrentUser();

        _logger.LogDebug("Checking if the guest id from the repository matches the guest id from the booking");
        if (guest.Id != booking.GuestId)
        {
            throw new UnauthorizedException(userId, booking.GuestId);
        }

        // check if the check-in time has passed
        if (booking.CheckInDate < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new BadRequestException("Cannot delete a booking that has started");
        }
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


    public async Task<BookingOutputModel> CreateBookingAsync(CreateBookingCommand request)
    {
        await _bookingRepository.BeginTransactionAsync();

        try
        {
            var booking = await ValidateAndCreateBooking(request);
            await _bookingRepository.AddBookingAsync(booking);
            await _bookingRepository.SaveChangesAsync();
            var outputModel = await PostBookingProcess(booking, _currentUser.Email);

            await _bookingRepository.CommitTransactionAsync();
            return outputModel;
        }
        catch (Exception ex)
        {
            await _bookingRepository.RollbackTransactionAsync();
            _logger.LogError(ex, "Error occurred during booking creation.");
            throw;
        }
    }

    private async Task<Booking> ValidateAndCreateBooking(CreateBookingCommand request)
    {
        var hotel = await _hotelRepository.GetHotelAsync(request.HotelId)
                ?? throw new NotFoundException(nameof(Hotel), request.HotelId);

        var (guest, _) = await GetGuestFromCurrentUser();

        var rooms = await FetchRooms(request);
        await ValidateRooms(request, rooms, hotel); 

        var checkInDate = DateOnly.FromDateTime(request.CheckInDate);
        var checkOutDate = DateOnly.FromDateTime(request.CheckOutDate);

        var totalPrice = await CalculateTotalPrice(rooms, checkInDate, checkOutDate);

        var booking = new Booking(guest, hotel, rooms)
        {
            NumberOfAdults = request.NumberOfAdults,
            NumberOfChildren = request.NumberOfChildren,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate,
            Price = totalPrice,
        };

        return booking;
    }

    private async Task ValidateRooms(CreateBookingCommand request, List<Room> rooms, Hotel hotel)
    {
        foreach (var room in rooms)
        {
            if (room == null)
            {
                throw new NotFoundException(nameof(Room));
            }
            ValidateRoomInHotel(room, hotel.Id);
            await ValidateRoomAvailability(room.Id, DateOnly.FromDateTime(request.CheckInDate), DateOnly.FromDateTime(request.CheckOutDate));
        }

        ValidateRoomsCapacity(request, rooms.Sum(r => r.AdultsCapacity), rooms.Sum(r => r.ChildrenCapacity));
    }

    private async Task<List<Room>> FetchRooms(CreateBookingCommand request)
    {
        var rooms = new List<Room>();

        foreach (var roomId in request.RoomIds)
        {
            var room = await _roomRepository.GetRoomAsync(roomId)
                ?? throw new ServerErrorException("Rooms cannot be null");
            rooms.Add(room);
        }

        return rooms;
    }

    private static void ValidateRoomInHotel(Room room, Guid hotelId)
    {
        if (room.HotelId != hotelId)
        {
            throw new BadRequestException($"Room with ID: {room.Id} does not belong to hotel with ID: {hotelId}");
        }
    }

    private async Task ValidateRoomAvailability(Guid roomId,
        DateOnly checkInDate, DateOnly checkOutDate)
    {
        _logger.LogDebug("Checking availability for RoomId: {RoomId}, CheckIn: {CheckIn}, CheckOut: {CheckOut}", roomId, checkInDate, checkOutDate);

        var isRoomAvailable = await _roomRepository.IsAvailableAsync(roomId, checkInDate, checkOutDate);
        if (!isRoomAvailable)
        {
            throw new UnavailableRoomException(roomId, checkInDate, checkOutDate);
        }

        _logger.LogDebug("RoomId: {RoomId} is available for the given dates, CheckIn: {CheckIn}, CheckOut: {CheckOut}", roomId, checkInDate, checkOutDate);
    }

    private void ValidateRoomsCapacity(CreateBookingCommand request,
        int totalAdultsCapacity, int totalChildrenCapacity)
    {
        if (totalAdultsCapacity < request.NumberOfAdults)
        {
            throw new InvalidNumberOfGuestsException($"Total adults capacity of the rooms: {totalAdultsCapacity} is less than the number of adults: {request.NumberOfAdults}");
        }

        if (totalChildrenCapacity < request.NumberOfChildren)
        {
            throw new InvalidNumberOfGuestsException($"Total children capacity of the rooms: {totalChildrenCapacity} is less than the number of children: {request.NumberOfChildren}");
        }

        _logger.LogDebug("Rooms capacity validated for request: {@CreateBookingCommand}", request);
    }


    private async Task<decimal> CalculateTotalPrice(List<Room> rooms,
        DateOnly checkInDate, DateOnly checkOutDate)
    {
        var numberOfNights = checkOutDate.DayNumber - checkInDate.DayNumber;

        var roomsPricePerNight = new List<decimal>();

        foreach (var room in rooms)
        {
            var price = await _roomRepository.GetPrice(room.Id, checkInDate, checkOutDate)
                ?? throw new NoPriceException(room.Id);
            roomsPricePerNight.Add(price);
        }

        var totalRoomsPricePerNight = roomsPricePerNight.Sum();

        var totalPrice = totalRoomsPricePerNight * numberOfNights;

        return totalPrice;
    }

    private async Task<BookingOutputModel> PostBookingProcess(Booking booking, string userEmail)
    {
        var outputModel = _mapper.Map<BookingOutputModel>(booking);

        await SendEmailAsync(booking, userEmail);

        return outputModel;
    }

    private async Task SendEmailAsync(Booking booking, string toEmail)
    {

        _logger.LogDebug("Converting booking to invoice for PDF generation. Booking: {booking}", booking);
        var invoice = await ConvertBookingToInvoiceAsync(booking);

        _logger.LogInformation("Preparing to send invoice: {@invoice} as email to {Email}", invoice, toEmail);

        _logger.LogInformation("Getting invoice pdf");
        var invoicePdf = GetInvoicePdf(invoice);

        _logger.LogDebug("Creating the mail instance for invoice: {@invoice}", invoice);
        MailRequest mail = new()
        {
            Body = "Thank You For Using Our Platform!",
            Subject = "Successful Booking",
            ToEmail = toEmail,
            Attachment = invoicePdf
        };

        _logger.LogDebug("Sending mail: {@mail} to {@toEmail} with emailService", mail, toEmail);
        await _emailService.SendEmailAsync(mail);

        _logger.LogDebug("Email sent successfully to {Email}", toEmail);
    }

    private async Task SendEmailAsync(Invoice invoice, string toEmail)
    {
        _logger.LogInformation("Preparing to send invoice: {@invoice} as email to {Email}", invoice, toEmail);

        _logger.LogInformation("Getting invoice pdf");
        var invoicePdf = GetInvoicePdf(invoice);

        _logger.LogDebug("Creating the mail instance for invoice: {@invoice}", invoice);
        MailRequest mail = new()
        {
            Body = "Thank You For Using Our Platform!",
            Subject = "Successful Booking",
            ToEmail = toEmail,
            Attachment = invoicePdf
        };

        _logger.LogInformation("Sending mail: {@mail} to {@toEmail} with emailService", mail, toEmail);
        await _emailService.SendEmailAsync(mail);

        _logger.LogInformation("Email sent successfully to {Email}", toEmail);
    }

    private byte[] GetInvoicePdf(Invoice invoice)
    {
        _logger.LogDebug("Generating PDF for Invoice with Id: {bookingId}", invoice.ConfirmationId);

        var pdfBytes = _pdfService.GeneratePdf(invoice);

        _logger.LogInformation("PDF generated successfully for Invoice with Id: {bookingId}", invoice.ConfirmationId);
        return pdfBytes;
    }


    public async Task<byte[]> GetInvoicePdfByBookingIdAsync(Guid bookingId)
    {
        _logger.LogInformation("GetInvoicePdf started for booking ID: {bookingId}", bookingId);

        _logger.LogDebug("Fetching booking for PDF generation. BookingId: {bookingId}", bookingId);
        var booking = await _bookingRepository.GetBookingAsync(bookingId)
            ?? throw new NotFoundException(nameof(Booking), bookingId);

        _logger.LogDebug("Checking if the Current guest matches the guest from the booking");
        var (guest, userId) = await GetGuestFromCurrentUser();
        if (booking.GuestId != guest.Id)
        {
            throw new UnauthorizedException(userId, booking.GuestId);
        }

        _logger.LogDebug("Converting booking to invoice for PDF generation. BookingId: {bookingId}", bookingId);
        Invoice invoice = await ConvertBookingToInvoiceAsync(booking);

        _logger.LogDebug("Generating PDF for BookingId: {bookingId}", bookingId);
        var pdfBytes = _pdfService.GeneratePdf(invoice);

        _logger.LogInformation("PDF generated successfully for BookingId: {bookingId}", bookingId);
        return pdfBytes;
    }

    public async Task<bool> DeleteBookingAsync(Guid bookingId)
    {
        _logger.LogInformation("DeleteBookingAsync started for booking ID: {BookingId}", bookingId);

        _logger.LogDebug("Retrieving booking with ID: {BookingId}", bookingId);
        var booking = await _bookingRepository.GetBookingAsync(bookingId);
        if(booking == null)
        {
            return false; 
        }

        await CanGuestDeleteTheBooking(booking);

        _logger.LogInformation("Deleting booking with ID from the Repository: {BookingId}", bookingId);
        await _bookingRepository.DeleteBookingAsync(bookingId);
        await _bookingRepository.SaveChangesAsync();

        _logger.LogInformation("DeleteBookingAsync completed successfully. Booking ID: {BookingId}", bookingId);
        return true;
    }

}