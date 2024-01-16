using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.EmailInterfaces;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.PdfInterfaces;
using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions;
using HotelBookingSystem.Application.DTOs.Booking.Command;
using HotelBookingSystem.Application.DTOs.Booking.OutputModel;

namespace HotelBookingSystem.Application.Tests.BookingTests;

/// <summary>
/// Tests for <see cref="BookingService.CreateBookingAsync(CreateBookingCommand)"/>
/// </summary>
public class BookingServiceBookingCreationTests
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

    public BookingServiceBookingCreationTests()
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
    public async Task CreateBookingAsync_ShouldPersistBookingAndReturnBookingConfirmation_WhenBookingOneRoomRequestIsValid()
    {
        // Arrange
        var hotel = fixture.Create<Hotel>();
        var room = fixture.Build<Room>()
            .With(r => r.HotelId, hotel.Id)
            .With(r => r.ChildrenCapacity, 2)
            .With(r => r.AdultsCapacity, 2)
            .Create();
        var request = fixture.Build<CreateBookingCommand>()
            .With(x => x.RoomIds, [room.Id])
            .With(x => x.HotelId, hotel.Id)
            .With(x => x.CheckInDate, DateTime.Today.AddDays(2))
            .With(x => x.CheckOutDate, DateTime.Today.AddDays(5))
            .With(x => x.NumberOfChildren, 1)
            .With(x => x.NumberOfAdults, 1)
            .Create();

        var guest = fixture.Create<Guest>();

        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(It.IsAny<string>())).ReturnsAsync(guest);
        hotelRepositoryMock.Setup(x => x.GetHotelAsync(request.HotelId)).ReturnsAsync(hotel);
        roomRepositoryMock.Setup(x => x.GetRoomAsync(room.Id)).ReturnsAsync(room);
        roomRepositoryMock.Setup(x => x.GetPrice(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>())).ReturnsAsync(1);
        roomRepositoryMock.Setup(x => x.IsAvailableAsync(room.Id,
            DateOnly.FromDateTime(request.CheckInDate),
            DateOnly.FromDateTime(request.CheckOutDate))).ReturnsAsync(true);
        bookingRepositoryMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
        bookingRepositoryMock.Setup(x => x.CommitTransactionAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await sut.CreateBookingAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BookingOutputModel>(result);
        bookingRepositoryMock.Verify(x => x.AddBookingAsync(It.IsAny<Booking>()), Times.Once);
        bookingRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }


    [Fact]
    public async Task CreateBookingAsync_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var request = fixture.Create<CreateBookingCommand>();
        hotelRepositoryMock.Setup(x => x.GetHotelAsync(request.HotelId)).ReturnsAsync((Hotel?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.CreateBookingAsync(request));
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldThrowNotFoundException_WhenRoomDoesNotExist()
    {
        // Arrange
        var request = fixture.Create<CreateBookingCommand>();
        var hotel = fixture.Create<Hotel>();

        hotelRepositoryMock.Setup(x => x.GetHotelAsync(request.HotelId)).ReturnsAsync(hotel);
        roomRepositoryMock.Setup(x => x.GetRoomAsync(It.IsAny<Guid>())).ReturnsAsync((Room?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.CreateBookingAsync(request));
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldThrowUnavailableRoomException_WhenRoomIsNotAvailable()
    {
        // Arrange
        var hotel = fixture.Create<Hotel>();
        var room = fixture.Build<Room>()
            .With(r => r.HotelId, hotel.Id)
            .Create();
        var request = fixture.Build<CreateBookingCommand>()
            .With(x => x.RoomIds, [room.Id])
            .With(x => x.HotelId, hotel.Id)
            .Create();

        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new Guest());
        hotelRepositoryMock.Setup(x => x.GetHotelAsync(request.HotelId)).ReturnsAsync(hotel);
        roomRepositoryMock.Setup(x => x.GetRoomAsync(room.Id)).ReturnsAsync(room);
        roomRepositoryMock.Setup(x => x.IsAvailableAsync(room.Id, DateOnly.FromDateTime(request.CheckInDate), DateOnly.FromDateTime(request.CheckOutDate))).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnavailableRoomException>(() => sut.CreateBookingAsync(request));
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    public async Task CreateBookingAsync_ShouldThrowInvalidNumberOfGuestsException_WhenRoomCapacityIsExceeded(int numberOfAdults, int numberOfChildren)
    {
        // Arrange
        var hotel = fixture.Create<Hotel>();
        var room = fixture.Build<Room>()
            .With(r => r.HotelId, hotel.Id)
            .With(r => r.ChildrenCapacity, 1)
            .With(r => r.AdultsCapacity, 1)
            .Create();
        var request = fixture.Build<CreateBookingCommand>()
            .With(x => x.RoomIds, [room.Id])
            .With(x => x.HotelId, hotel.Id)
            .With(x => x.NumberOfAdults, numberOfAdults)
            .With(x => x.NumberOfChildren, numberOfChildren)
            .Create();

        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new Guest());
        hotelRepositoryMock.Setup(x => x.GetHotelAsync(request.HotelId)).ReturnsAsync(hotel);
        roomRepositoryMock.Setup(x => x.GetRoomAsync(room.Id)).ReturnsAsync(room);
        roomRepositoryMock.Setup(x => x.IsAvailableAsync(room.Id, DateOnly.FromDateTime(request.CheckInDate), DateOnly.FromDateTime(request.CheckOutDate))).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidNumberOfGuestsException>(() => sut.CreateBookingAsync(request));
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldRollbackTransaction_WhenExceptionOccurs()
    {
        // Arrange
        var request = fixture.Create<CreateBookingCommand>();
        hotelRepositoryMock.Setup(x => x.GetHotelAsync(request.HotelId)).ThrowsAsync(new Exception("Database error"));

        bookingRepositoryMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
        bookingRepositoryMock.Setup(x => x.RollbackTransactionAsync()).Returns(Task.CompletedTask);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => sut.CreateBookingAsync(request));
        bookingRepositoryMock.Verify(x => x.RollbackTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldThrowNotFoundException_WhenGuestDoesNotExist()
    {
        // Arrange
        var request = fixture.Create<CreateBookingCommand>();
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(It.IsAny<string>())).ReturnsAsync((Guest?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.CreateBookingAsync(request));
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldThrowNoPriceException_WhenRoomPriceIsNotAvailable()
    {
        // Arrange
        var hotel = fixture.Create<Hotel>();
        var room = fixture.Build<Room>()
            .With(r => r.HotelId, hotel.Id)
            .With(r => r.ChildrenCapacity, 2)
            .With(r => r.AdultsCapacity, 2)
            .Create();
        var request = fixture.Build<CreateBookingCommand>()
            .With(x => x.RoomIds, [room.Id])
            .With(x => x.HotelId, hotel.Id)
            .With(x => x.CheckInDate, DateTime.Today.AddDays(2))
            .With(x => x.CheckOutDate, DateTime.Today.AddDays(5))
            .With(x => x.NumberOfChildren, 1)
            .With(x => x.NumberOfAdults, 1)
            .Create();

        var guest = fixture.Create<Guest>();

        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(It.IsAny<string>())).ReturnsAsync(guest);
        hotelRepositoryMock.Setup(x => x.GetHotelAsync(request.HotelId)).ReturnsAsync(hotel);
        roomRepositoryMock.Setup(x => x.GetRoomAsync(room.Id)).ReturnsAsync(room);
        roomRepositoryMock.Setup(x => x.GetPrice(room.Id,
           DateOnly.FromDateTime(request.CheckInDate),
           DateOnly.FromDateTime(request.CheckOutDate))).ReturnsAsync((decimal?)null);
        roomRepositoryMock.Setup(x => x.IsAvailableAsync(room.Id,
            DateOnly.FromDateTime(request.CheckInDate),
            DateOnly.FromDateTime(request.CheckOutDate))).ReturnsAsync(true);


        // Act & Assert
        await Assert.ThrowsAsync<NoPriceException>(() => sut.CreateBookingAsync(request));
    }
}
