using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.EmailInterfaces;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.PdfInterfaces;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions;
using HotelBookingSystem.Application.DTOs.Booking.OutputModel;

namespace HotelBookingSystem.Application.Tests.BookingTests;
/// <summary>
/// Tests for <see cref="BookingService.GetBookingAsync(Guid)"/>,
/// <see cref="BookingService.GetInvoicePdfByBookingIdAsync(Guid)"/> and
/// <see cref="BookingService.DeleteBookingAsync(Guid)"/> methods
/// </summary>
public class BookingServiceTests
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

    public BookingServiceTests()
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
    public async Task GetBookingAsync_ShouldReturnBookingOutputModel_WhenBookingExists()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var booking = fixture.Create<Booking>();
        var invoice = fixture.Create<Invoice>();

        MockCurrentUser(booking);
        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync(booking);

        // Act
        var result = await sut.GetBookingAsync(bookingId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BookingOutputModel>(result);
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
    public async Task GetBookingAsync_ShouldThrowNotFoundException_WhenBookingDoesNotExist()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync((Booking?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.GetBookingAsync(bookingId));
        bookingRepositoryMock.Verify(x => x.GetBookingAsync(bookingId), Times.Once);
    }

    [Fact]
    public async Task GetBookingAsync_ShouldThrowUnauthorizedException_WhenCurrentUserIsNotGuestOfBooking()
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
        await Assert.ThrowsAsync<UnauthorizedException>(() => sut.GetBookingAsync(bookingId));
    }

    [Fact]
    public async Task GetInvoicePdf_ShouldReturnPdfBytes_WhenBookingExistsAndUserIsAuthorized()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var booking = fixture.Build<Booking>()
                             .With(b => b.Id, bookingId)
                             .Create();

        MockCurrentUser(booking);
        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync(booking);
        pdfGeneratorMock.Setup(x => x.GeneratePdf(It.IsAny<Invoice>())).Returns([1, 2, 3, 4]); // Sample bytes
        roomRepositoryMock.Setup(x => x.GetPrice(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>())).ReturnsAsync(1);

        // Act
        var result = await sut.GetInvoicePdfByBookingIdAsync(bookingId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<byte[]>(result);
        bookingRepositoryMock.Verify(x => x.GetBookingAsync(bookingId), Times.Once);
        pdfGeneratorMock.Verify(x => x.GeneratePdf(It.IsAny<Invoice>()), Times.Once);
    }

    [Fact]
    public async Task GetInvoicePdf_ShouldThrowNotFoundException_WhenBookingDoesNotExist()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync((Booking?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.GetInvoicePdfByBookingIdAsync(bookingId));
    }

    [Fact]
    public async Task GetInvoicePdf_ShouldThrowUnauthorizedException_WhenCurrentUserIsNotGuestOfBooking()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var unauthorizedGuest = fixture.Create<Guest>();
        var booking = fixture.Create<Booking>(); // Booking will have a different GuestId
        var unauthorizedUserId = "unauthorized_user_id";

        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync(booking);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(unauthorizedUserId)).ReturnsAsync(unauthorizedGuest);
        currentUserMock.SetupGet(x => x.Id).Returns(unauthorizedUserId);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => sut.GetInvoicePdfByBookingIdAsync(bookingId));
    }

    [Fact]
    public async Task DeleteBookingAsync_ShouldReturnTrue_WhenUserIsAuthorizedAndBookingIsSuccessfullyDeleted()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var guestId = Guid.NewGuid();

        var booking = fixture.Build<Booking>()
                             .With(b => b.Id, bookingId)
                             .With(b => b.GuestId, guestId)
                             .With(b => b.CheckInDate, DateOnly.FromDateTime(DateTime.Today.AddDays(2)))
                             .With(b => b.CheckOutDate, DateOnly.FromDateTime(DateTime.Today.AddDays(5)))
                             .Create();

        var guest = fixture.Build<Guest>()
            .With(g => g.Id, guestId)
            .With(g => g.Bookings, new List<Booking>() { booking })
            .Create();

        string userId = "user_id";
        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync(booking);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(userId)).ReturnsAsync(guest);
        currentUserMock.SetupGet(x => x.Id).Returns(userId);

        // Act
        var result = await sut.DeleteBookingAsync(bookingId);

        // Assert
        Assert.True(result);
        bookingRepositoryMock.Verify(x => x.DeleteBookingAsync(bookingId), Times.Once);
        bookingRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteBookingAsync_ShouldThrowUnauthorizedException_WhenCurrentUserIsNotGuestOfBooking()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        var unauthorizedUserId = "unauthorized_user_id";

        var booking = fixture.Build<Booking>()
                             .With(b => b.Id, bookingId)
                             .With(b => b.GuestId, guestId)
                             .Create();
        var unauthorizedGuest = fixture.Build<Guest>()
                                       .With(g => g.Id, Guid.NewGuid()) // Different guest ID
                                       .Create();

        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync(booking);
        currentUserMock.SetupGet(x => x.Id).Returns(unauthorizedUserId);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(unauthorizedUserId)).ReturnsAsync(unauthorizedGuest);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => sut.DeleteBookingAsync(bookingId));
    }

    [Fact]
    public async Task DeleteBookingAsync_ShouldReturnFalse_WhenBookingDoesNotExist()
    {
        // Arrange
        var bookingId = Guid.NewGuid();

        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync((Booking?)null);

        // Act & Assert
        var result = await sut.DeleteBookingAsync(bookingId);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteBookingAsync_ShouldThrowBadRequestException_WhenTryingToDeleteBookingAfterCheckIn()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        var userId = "user_id";

        var booking = fixture.Build<Booking>()
                             .With(b => b.Id, bookingId)
                             .With(b => b.GuestId, guestId)
                             .With(b => b.CheckInDate, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))) // Booking has already started
                             .Create();
        var guest = fixture.Build<Guest>()
                           .With(g => g.Id, guestId)
                           .Create();

        bookingRepositoryMock.Setup(x => x.GetBookingAsync(bookingId)).ReturnsAsync(booking);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(userId)).ReturnsAsync(guest);
        currentUserMock.SetupGet(x => x.Id).Returns(userId);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => sut.DeleteBookingAsync(bookingId));
    }

}
