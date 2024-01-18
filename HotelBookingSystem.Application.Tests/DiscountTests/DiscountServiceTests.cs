using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.DTOs.Discount;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Application.Tests.DiscountTests;

/// <summary>
/// Tests for <see cref="DiscountService.DeleteDiscountAsync(Guid, Guid)"/>, 
/// <see cref="DiscountService.GetDiscountAsync(Guid, Guid)"/> and
/// <see cref="DiscountService.GetFeaturedDealsAsync(int)"/>
/// </summary>
public class DiscountServiceTests
{
    private readonly DiscountService sut;
    private readonly Mock<IDiscountRepository> discountRepositoryMock;
    private readonly Mock<IRoomRepository> roomRepositoryMock;
    private readonly IMapper mapper;
    private readonly IFixture fixture;

    public DiscountServiceTests()
    {
        fixture = FixtureFactory.CreateFixture();
        mapper = AutoMapperSingleton.Mapper;
        discountRepositoryMock = new Mock<IDiscountRepository>();
        roomRepositoryMock = new Mock<IRoomRepository>();
        var logger = new Mock<ILogger<DiscountService>>();
        sut = new DiscountService(roomRepositoryMock.Object,
            discountRepositoryMock.Object,
            mapper,
            logger.Object
            );
    }

    [Fact]
    public async Task DeleteDiscountAsync_ShouldReturnTrue_WhenDiscountDeleted()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var discountId = Guid.NewGuid();

        discountRepositoryMock.Setup(x => x.DeleteDiscountAsync(roomId, discountId)).ReturnsAsync(true);

        // Act
        var result = await sut.DeleteDiscountAsync(roomId, discountId);

        // Assert
        Assert.True(result);
        discountRepositoryMock.Verify(x => x.DeleteDiscountAsync(roomId, discountId), Times.Once);
        discountRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteDiscountAsync_ShouldReturnFalse_WhenDiscountNotDeleted()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var discountId = Guid.NewGuid();

        discountRepositoryMock.Setup(x => x.DeleteDiscountAsync(roomId, discountId)).ReturnsAsync(false);

        // Act
        var result = await sut.DeleteDiscountAsync(roomId, discountId);

        // Assert
        Assert.False(result);
        discountRepositoryMock.Verify(x => x.DeleteDiscountAsync(roomId, discountId), Times.Once);
        discountRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task GetDiscountAsync_ShouldReturnDiscount_WhenDiscountExists()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var discountId = Guid.NewGuid();
        var discount = fixture.Create<Discount>();
        discountRepositoryMock.Setup(x => x.GetDiscountAsync(roomId, discountId)).ReturnsAsync(discount);

        // Act
        var result = await sut.GetDiscountAsync(roomId, discountId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<DiscountOutputModel>(result);
        discountRepositoryMock.Verify(x => x.GetDiscountAsync(roomId, discountId), Times.Once);
    }


    [Fact]
    public async Task GetDiscountAsync_ShouldThrowNotFoundException_WhenDiscountDoesNotExist()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var discountId = Guid.NewGuid();
        discountRepositoryMock.Setup(x => x.GetDiscountAsync(roomId, discountId)).ReturnsAsync((Discount?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.GetDiscountAsync(roomId, discountId));
        discountRepositoryMock.Verify(x => x.GetDiscountAsync(roomId, discountId), Times.Once);
    }

    [Fact]
    public async Task GetFeaturedDealsAsync_ShouldReturnFeaturedDeals_WhenDealsExist()
    {
        // Arrange
        var dealsCount = 2;
        var discount1 = fixture.Create<Discount>();
        var discount2 = fixture.Create<Discount>();
        var room1 = fixture.Build<Room>()
                    .With(r => r.Discounts, [discount1])
                    .Create();

        var room2 = fixture.Build<Room>()
                    .With(r => r.Discounts, [discount2])
                    .Create();

        var rooms = new List<Room> { room1, room2}; 

        roomRepositoryMock.Setup(x => x.GetRoomsWithHighestDiscounts(dealsCount)).ReturnsAsync(rooms);

        // Act
        var result = await sut.GetFeaturedDealsAsync(dealsCount);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dealsCount, result.Count());
        roomRepositoryMock.Verify(x => x.GetRoomsWithHighestDiscounts(dealsCount), Times.Once);
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(21)]
    [InlineData(22)]
    public async Task GetFeaturedDealsAsync_ShouldThrowBadRequestException_WhenInvalidDealsCount(
        int invalidDeals)
    {
        // Arrange
        var deals = invalidDeals;

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => sut.GetFeaturedDealsAsync(deals));
        roomRepositoryMock.Verify(x => x.GetRoomsWithHighestDiscounts(It.IsAny<int>()), Times.Never);
    }


}
