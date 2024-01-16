using HotelBookingSystem.Application.Abstractions;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.EmailInterfaces;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.PdfInterfaces;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.DTOs.Booking.OutputModel;

namespace HotelBookingSystem.Application.Tests.BookingTests;
/// <summary>
/// Tests for Invoice Calculations in <see cref="BookingService.GetInvoiceAsync(Guid)"/>
/// </summary>
public class BookingServiceInvoiceCalculationsTests
{
    private readonly Mock<IHotelRepository> hotelRepositoryMock;
    private readonly Mock<IRoomRepository> roomRepositoryMock;
    private readonly Mock<IGuestRepository> guestRepositoryMock;
    private readonly Mock<IBookingRepository> bookingRepositoryMock;
    private readonly Mock<IPdfGenerator> pdfGeneratorMock;
    private readonly Mock<IEmailService> emailServiceMock;
    private readonly Mock<ICurrentUser> currentUserMock;
    private readonly IMapper mapper;
    private readonly Mock<ILogger<BookingService>> loggerMock;
    private readonly IFixture fixture;
    private readonly BookingService sut;

    public BookingServiceInvoiceCalculationsTests()
    {
        fixture = FixtureFactory.CreateFixture();
        hotelRepositoryMock = new Mock<IHotelRepository>();
        roomRepositoryMock = new Mock<IRoomRepository>();
        guestRepositoryMock = new Mock<IGuestRepository>();
        bookingRepositoryMock = new Mock<IBookingRepository>();
        pdfGeneratorMock = new Mock<IPdfGenerator>();
        emailServiceMock = new Mock<IEmailService>();
        currentUserMock = new Mock<ICurrentUser>();
        mapper = AutoMapperSingleton.Mapper;
        loggerMock = new Mock<ILogger<BookingService>>();

        sut = new BookingService(hotelRepositoryMock.Object,
                                 roomRepositoryMock.Object,
                                 guestRepositoryMock.Object,
                                 bookingRepositoryMock.Object,
                                 mapper,
                                 currentUserMock.Object,
                                 pdfGeneratorMock.Object,
                                 emailServiceMock.Object,
                                 loggerMock.Object);
    }

    [Fact]
    public async Task GetInvoiceAsync_ShouldReturnAnInvoice_WhenBookingExists()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var booking = fixture.Create<Booking>();
        var invoice = fixture.Create<Invoice>();

        MockCurrentUser(booking);
        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync(booking);
        roomRepositoryMock.Setup(x => x.GetPrice(It.IsAny<Guid>(), booking.CheckInDate, booking.CheckOutDate)).ReturnsAsync(1);

        // Act
        var result = await sut.GetInvoiceAsync(bookingId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Invoice>(result);
        bookingRepositoryMock.Verify(x => x.GetBookingAsync(bookingId), Times.Once);
    }

    private void MockCurrentUser(Booking booking)
    {
        var guestId = Guid.NewGuid();
        var guest = fixture.Build<Guest>()
                           .With(g => g.Id, guestId)
                           .Create();
        var userId = "userId";
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(userId)).ReturnsAsync(guest);
        currentUserMock.Setup(x => x.Id).Returns(userId);
        booking.GuestId = guestId;
    }

    [Fact]
    public async Task GetInvoiceAsync_ShouldCalculateCorrectNumberOfNightsInTheReturnedInvoice_WhenBookingOneRoom()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var booking = fixture.Build<Booking>()
                             .With(b => b.CheckInDate, DateOnly.FromDateTime(DateTime.Today))
                             .With(b => b.CheckOutDate, DateOnly.FromDateTime(DateTime.Today.AddDays(3)))
                             .With(b => b.Rooms, new List<Room> { new() { Id = Guid.NewGuid() } })
                             .Create();
        var roomId = booking.Rooms.First().Id;
        var expectedNumberOfNights = 3;

        MockCurrentUser(booking);
        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync(booking);
        roomRepositoryMock.Setup(x => x.GetPrice(booking.Rooms.First().Id, booking.CheckInDate, booking.CheckOutDate)).ReturnsAsync(1);

        // Act
        var result = await sut.GetInvoiceAsync(bookingId);
        Assert.NotNull(result);
        var firstRoomInvoice = result.Rooms.First();

        // Assert
        Assert.Equal(expectedNumberOfNights, firstRoomInvoice.NumberOfNights);
    }

    [Fact]
    public async Task GetInvoiceAsync_ShouldCalculateCorrectRoomPricesInTheReturnedInvoice_WhenBookingOneRoomWithoutDiscount()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var roomPricePerNight = 100m;
        var numberOfNights = 3;
        var totalRoomPrice = roomPricePerNight * numberOfNights;
        var booking = fixture.Build<Booking>()
                             .With(b => b.CheckInDate, DateOnly.FromDateTime(DateTime.Today))
                             .With(b => b.CheckOutDate, DateOnly.FromDateTime(DateTime.Today.AddDays(numberOfNights)))
                             .With(b => b.Rooms, new List<Room> { new() { Id = Guid.NewGuid(), Price = roomPricePerNight } })
                             .With(b => b.Price, totalRoomPrice)
                             .Create();

        MockCurrentUser(booking);
        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync(booking);
        roomRepositoryMock.Setup(x => x.GetPrice(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>())).ReturnsAsync(roomPricePerNight); // No discount

        // Act
        var result = await sut.GetInvoiceAsync(bookingId);
        Assert.NotNull(result);
        var roomInvoice = result.Rooms.First();

        // Assert
        Assert.Equal(totalRoomPrice, result.TotalPrice);
        Assert.Equal(totalRoomPrice, result.TotalPriceAfterDiscount);  // Total price should be the same as no discount

        Assert.NotNull(roomInvoice);
        Assert.Equal(roomPricePerNight, roomInvoice.PricePerNight);
        Assert.Equal(roomPricePerNight, roomInvoice.PricePerNightAfterDiscount);
        Assert.Equal(totalRoomPrice, roomInvoice.TotalRoomPrice);
        Assert.Equal(totalRoomPrice, roomInvoice.TotalRoomPriceAfterDiscount);
    }

    [Fact]
    public async Task GetInvoiceAsync_ShouldCalculateCorrectRoomPricesInTheReturnedInvoice_WhenBookingMultipleRoomsWithoutDiscount()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var numberOfNights = 3;
        var rooms = new List<Room>()
                    {
                        new() { Id = Guid.NewGuid(), Price = 10 },
                        new() { Id = Guid.NewGuid(), Price = 20 }
                    };
        var totalBookingPrice = rooms.Sum(room => room.Price * numberOfNights);
        var booking = fixture.Build<Booking>()
                             .With(b => b.CheckInDate, DateOnly.FromDateTime(DateTime.Today))
                             .With(b => b.CheckOutDate, DateOnly.FromDateTime(DateTime.Today.AddDays(numberOfNights)))
                             .With(b => b.Rooms, rooms)
                             .With(b => b.Price, totalBookingPrice)
                             .Create();
        MockCurrentUser(booking);
        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync(booking);
        foreach (var room in rooms)
        {
            roomRepositoryMock.Setup(x => x.GetPrice(room.Id, booking.CheckInDate, booking.CheckOutDate))
                .ReturnsAsync(room.Price);
        }

        // Act
        var result = await sut.GetInvoiceAsync(bookingId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(totalBookingPrice, result.TotalPrice);
        Assert.Equal(totalBookingPrice, result.TotalPriceAfterDiscount); // Total price should be the same as no discount
        foreach (var roomInvoice in result.Rooms)
        {
            var originalRoom = rooms.First(r => r.Id == roomInvoice.Id);
            Assert.Equal(originalRoom.Price, roomInvoice.PricePerNight);
            Assert.Equal(originalRoom.Price, roomInvoice.PricePerNightAfterDiscount);
            Assert.Equal(originalRoom.Price * numberOfNights, roomInvoice.TotalRoomPrice);
            Assert.Equal(originalRoom.Price * numberOfNights, roomInvoice.TotalRoomPriceAfterDiscount);
        }
    }


    [Fact]
    public async Task GetInvoiceAsync_ShouldCalculateCorrectRoomPricesInTheReturnedInvoice_WhenBookingOneRoomWithDiscount()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var booking = fixture.Build<Booking>()
                             .With(b => b.CheckInDate, DateOnly.FromDateTime(DateTime.Today))
                             .With(b => b.CheckOutDate, DateOnly.FromDateTime(DateTime.Today.AddDays(3)))
                             .With(b => b.Rooms, new List<Room> { new() { Id = Guid.NewGuid(), Price = 100 } })
                             .With(b => b.Price, 240)
                             .Create();

        var roomId = booking.Rooms.First().Id;
        var expectedNumberOfNights = 3;
        var expectedRoomPricePerNight = 100m;
        var expectedRoomDiscountedPrice = 80m; // Assuming a discount to $80 per night
        var expectedTotalRoomPrice = expectedRoomPricePerNight * expectedNumberOfNights;
        var expectedTotalRoomPriceAfterDiscount = expectedRoomDiscountedPrice * expectedNumberOfNights;

        var expectedTotalPrice = expectedTotalRoomPrice;
        var expectedTotalPriceAfterDiscount = expectedTotalRoomPriceAfterDiscount;

        MockCurrentUser(booking);
        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync(booking);
        roomRepositoryMock.Setup(x => x.GetPrice(roomId, booking.CheckInDate, booking.CheckOutDate)).ReturnsAsync(expectedRoomDiscountedPrice);

        // Act
        var result = await sut.GetInvoiceAsync(bookingId);
        Assert.NotNull(result);
        var firstRoomInvoice = result.Rooms.First();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedRoomPricePerNight, firstRoomInvoice.PricePerNight);
        Assert.Equal(expectedRoomDiscountedPrice, firstRoomInvoice.PricePerNightAfterDiscount);
        Assert.Equal(expectedTotalRoomPrice, firstRoomInvoice.TotalRoomPrice);
        Assert.Equal(expectedTotalRoomPriceAfterDiscount, firstRoomInvoice.TotalRoomPriceAfterDiscount);
        Assert.Equal(expectedTotalPrice, result.TotalPrice);
        Assert.Equal(expectedTotalPriceAfterDiscount, result.TotalPriceAfterDiscount);
        bookingRepositoryMock.Verify(x => x.GetBookingAsync(bookingId), Times.Once);
        roomRepositoryMock.Verify(x => x.GetPrice(roomId, booking.CheckInDate, booking.CheckOutDate), Times.Once);
    }


    [Fact]
    public async Task GetInvoiceAsync_ShouldCalculateCorrectRoomPricesInTheReturnedInvoice_WhenBookingMultipleRoomsWithDiscount()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var rooms = new List<Room>()
                    {
                        new() { Id = Guid.NewGuid(), Price = 10 },
                        new() { Id = Guid.NewGuid(), Price = 20 }
                    };
        var booking = fixture.Build<Booking>()
                             .With(b => b.CheckInDate, DateOnly.FromDateTime(DateTime.Today))
                             .With(b => b.CheckOutDate, DateOnly.FromDateTime(DateTime.Today.AddDays(3)))
                             .With(b => b.Rooms, rooms)
                             .With(b => b.Price, 81)  // 9*3 + 18*3 = 81 (prices after 10% discount)
                             .Create();

        var expectedNumberOfNights = 3;
        var expectedTotalPrice = rooms.Sum(room => room.Price * expectedNumberOfNights);

        var expectedTotalPriceAfterDiscount = 0m;
        foreach (var room in rooms)
        {
            var discountedPrice = room.Price * 0.9m; // Assuming a 10% discount
            expectedTotalPriceAfterDiscount += discountedPrice * expectedNumberOfNights;
            roomRepositoryMock.Setup(x => x.GetPrice(room.Id, booking.CheckInDate, booking.CheckOutDate)).ReturnsAsync(discountedPrice);
        }

        MockCurrentUser(booking);
        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync(booking);

        // Act
        var result = await sut.GetInvoiceAsync(bookingId);
        Assert.NotNull(result);

        // Assert
        Assert.Equal(expectedTotalPrice, result.TotalPrice);
        Assert.Equal(expectedTotalPriceAfterDiscount, result.TotalPriceAfterDiscount);
        foreach (var roomInvoice in result.Rooms)
        {
            var originalRoom = rooms.First(r => r.Id == roomInvoice.Id);
            Assert.Equal(originalRoom.Price, roomInvoice.PricePerNight);
            Assert.Equal(originalRoom.Price * 0.9m, roomInvoice.PricePerNightAfterDiscount);
            Assert.Equal(originalRoom.Price * expectedNumberOfNights, roomInvoice.TotalRoomPrice);
            Assert.Equal(originalRoom.Price * 0.9m * expectedNumberOfNights, roomInvoice.TotalRoomPriceAfterDiscount);
        }

        bookingRepositoryMock.Verify(x => x.GetBookingAsync(bookingId), Times.Once);
        foreach (var room in rooms)
        {
            roomRepositoryMock.Verify(x => x.GetPrice(room.Id, booking.CheckInDate, booking.CheckOutDate), Times.Once);
        }
    }



    [Fact]
    public async Task GetInvoiceAsync_ShouldThrowUnauthorizedException_WhenCurrentUserIsNotGuestOfBooking()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var unauthorizedGuest = fixture.Create<Guest>();
        var booking = fixture.Create<Booking>(); // Booking will have a different GuestId
        var unauthorizedUserId = "unauthorized_user_id";

        currentUserMock.SetupGet(x => x.Id).Returns(unauthorizedUserId);
        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync(booking);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(unauthorizedUserId)).ReturnsAsync(unauthorizedGuest);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => sut.GetInvoiceAsync(bookingId));
    }

    [Fact]
    public async Task GetInvoiceAsync_ShouldThrowNotFoundException_WhenBookingDoesNotExist()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync((Booking?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.GetInvoiceAsync(bookingId));
        bookingRepositoryMock.Verify(x => x.GetBookingAsync(bookingId), Times.Once);
    }

    [Fact]
    public async Task GetInvoiceAsync_ShouldThrowNoPriceException_WhenFetchingRoomPriceFails()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var booking = fixture.Create<Booking>();
        var invoice = fixture.Create<Invoice>();

        MockCurrentUser(booking);
        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync(booking);
        roomRepositoryMock.Setup(x => x.GetPrice(It.IsAny<Guid>(), booking.CheckInDate, booking.CheckOutDate))
            .ReturnsAsync((decimal?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NoPriceException>(() => sut.GetInvoiceAsync(bookingId));
        bookingRepositoryMock.Verify(x => x.GetBookingAsync(bookingId), Times.Once);
    }


}

