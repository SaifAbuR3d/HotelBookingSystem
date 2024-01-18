using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;

namespace HotelBookingSystem.Application.Tests;

public class GuestServiceTests
{
    private readonly Mock<IGuestRepository> guestRepositoryMock;
    private readonly Mock<IBookingRepository> bookingRepositoryMock;
    private readonly Mock<IHotelRepository> hotelRepositoryMock;
    private readonly IMapper mapper;
    private readonly Mock<ICurrentUser> currentUserMock;
    private readonly Mock<ILogger<GuestService>> loggerMock;
    private readonly IFixture fixture;
    private readonly GuestService sut;

    public GuestServiceTests()
    {
        fixture = FixtureFactory.CreateFixture();
        guestRepositoryMock = new Mock<IGuestRepository>();
        bookingRepositoryMock = new Mock<IBookingRepository>();
        hotelRepositoryMock = new Mock<IHotelRepository>();
        currentUserMock = new Mock<ICurrentUser>();
        mapper = AutoMapperSingleton.Mapper;
        loggerMock = new Mock<ILogger<GuestService>>();

        sut = new GuestService(guestRepositoryMock.Object,
                               bookingRepositoryMock.Object,
                               hotelRepositoryMock.Object,
                               mapper,
                               currentUserMock.Object,
                               loggerMock.Object);
    }

    [Fact]
    public async Task GetRecentlyVisitedHotelsAsync_ShouldReturnHotels_WhenValidGuestIdAndCount()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var count = 5;
        var recentBookings = fixture.CreateMany<Booking>(count);
        guestRepositoryMock.Setup(x => x.GuestExistsAsync(guestId)).ReturnsAsync(true);
        guestRepositoryMock.Setup(x => x.GetRecentBookingsInDifferentHotelsAsync(guestId, count)).ReturnsAsync(recentBookings);

        // Act
        var result = await sut.GetRecentlyVisitedHotelsAsync(guestId, count);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<RecentlyVisitedHotelOutputModel>>(result);
        Assert.Equal(count, result.Count());
        guestRepositoryMock.Verify(x => x.GetRecentBookingsInDifferentHotelsAsync(guestId, count), Times.Once);
    }

    [Fact]
    public async Task GetRecentlyVisitedHotelsAsync_ShouldThrowNotFoundException_WhenGuestDoesNotExist()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var count = 5;
        guestRepositoryMock.Setup(x => x.GuestExistsAsync(guestId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.GetRecentlyVisitedHotelsAsync(guestId, count));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public async Task GetRecentlyVisitedHotelsAsync_ShouldThrowBadRequestException_WhenInvalidCount(int count)
    {
        // Arrange
        var guestId = Guid.NewGuid();
        guestRepositoryMock.Setup(x => x.GuestExistsAsync(guestId)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => sut.GetRecentlyVisitedHotelsAsync(guestId, count));
    }

    [Fact]
    public async Task GetRecentlyVisitedHotelsForCurrentUserAsync_ShouldReturnHotels_WhenValidCount()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var userId = "current_user_id";
        var count = 5;
        var guest = fixture.Build<Guest>()
            .With(g => g.Id, guestId)
            .Create();
        var recentBookings = fixture.CreateMany<Booking>(count);

        currentUserMock.Setup(x => x.Id).Returns(userId);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(userId)).ReturnsAsync(guest);
        guestRepositoryMock.Setup(x => x.GetRecentBookingsInDifferentHotelsAsync(guestId, count)).ReturnsAsync(recentBookings);

        // Act
        var result = await sut.GetRecentlyVisitedHotelsForCurrentUserAsync(count);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<RecentlyVisitedHotelOutputModel>>(result);
        Assert.Equal(count, result.Count());
        guestRepositoryMock.Verify(x => x.GetGuestByUserIdAsync(userId), Times.Once);
        guestRepositoryMock.Verify(x => x.GetRecentBookingsInDifferentHotelsAsync(guestId, count), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public async Task GetRecentlyVisitedHotelsForCurrentUserAsync_ShouldThrowBadRequestException_WhenInvalidCount(int count)
    {
        // Arrange
        var userId = "current_user_id";
        currentUserMock.Setup(x => x.Id).Returns(userId);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => sut.GetRecentlyVisitedHotelsForCurrentUserAsync(count));
    }

    [Fact]
    public async Task GetRecentlyVisitedHotelsForCurrentUserAsync_ShouldThrowNotFoundException_WhenGuestDoesNotExist()
    {
        // Arrange
        var userId = "current_user_id";
        currentUserMock.Setup(x => x.Id).Returns(userId);
        guestRepositoryMock.Setup(x => x.GetGuestByUserIdAsync(userId)).ReturnsAsync((Guest?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.GetRecentlyVisitedHotelsForCurrentUserAsync(5));
    }


}

