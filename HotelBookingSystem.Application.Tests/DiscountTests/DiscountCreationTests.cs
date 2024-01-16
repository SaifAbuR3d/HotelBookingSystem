using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.RepositoryInterfaces;
using HotelBookingSystem.Application.DTOs.Discount;

namespace HotelBookingSystem.Application.Tests.DiscountTests;

/// <summary>
/// Tests for <see cref="DiscountService.AddDiscountAsync(Guid, CreateDiscountCommand)"/>
/// <para>
/// and <see cref="Discount"/> Constructors: <see cref="Discount(Room, decimal, DateTime, DateTime)"/>
/// and <see cref="Discount(Room, decimal, decimal, DateTime, DateTime)"/>
/// and Discount Calculations
/// </para>
/// </summary>
public class DiscountCreationTests
{
    private readonly DiscountService sut;
    private readonly Mock<IDiscountRepository> discountRepositoryMock;
    private readonly Mock<IRoomRepository> roomRepositoryMock;
    private readonly IMapper mapper;
    private readonly IFixture fixture;

    public DiscountCreationTests()
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


    private CreateDiscountCommand GetValidCreateDiscountCommand()
    {
        return fixture.Build<CreateDiscountCommand>()
            .With(x => x.StartDate, DateTime.UtcNow.AddDays(1))
            .With(x => x.EndDate, DateTime.UtcNow.AddDays(2))
            .Create();
    }

    [Fact]
    public async Task AddDiscountAsync_ShouldCreateDiscount_WhenValidDataProvided()
    {
        // Arrange
        var command = GetValidCreateDiscountCommand();
        command.DiscountedPrice = 80m;

        var roomId = Guid.NewGuid();
        var room = fixture.Build<Room>()
            .With(x => x.Id, roomId)
            .With(x => x.Price, 100m)
            .Create();
        
        roomRepositoryMock.Setup(x => x.GetRoomAsync(roomId)).ReturnsAsync(room);

        // Act
        var result = await sut.AddDiscountAsync(roomId, command);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<DiscountOutputModel>(result);
        roomRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddDiscountAsync_ShouldReturnCorrectDiscountData_WhenDiscountedPriceProvided()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var room = fixture.Build<Room>()
            .With(x => x.Id, roomId)
            .With(x => x.Price, 100m)
            .Create();

        var discountedPrice = 80m;

        var command = GetValidCreateDiscountCommand();
        command.DiscountedPrice = discountedPrice;

        var expectedPercentage = (room.Price - discountedPrice) / room.Price * 100; // 20% discount
        roomRepositoryMock.Setup(x => x.GetRoomAsync(roomId)).ReturnsAsync(room);

        // Act
        var result = await sut.AddDiscountAsync(roomId, command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(roomId, result.RoomId);
        Assert.Equal(discountedPrice, result.DiscountedPrice);
        Assert.Equal(room.Price, result.OriginalPrice);
        Assert.Equal(expectedPercentage, result.Percentage);
        roomRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddDiscountAsync_ShouldReturnCorrectDiscountDataAndIgnorePercentage_WhenDiscountedPriceAndPercentageProvided()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var room = fixture.Build<Room>()
            .With(x => x.Id, roomId)
            .With(x => x.Price, 100m)
            .Create();

        var discountedPrice = 80m;


        var command = GetValidCreateDiscountCommand();
        command.DiscountedPrice = discountedPrice;
        command.Percentage = 50m;

        var expectedPercentage = (room.Price - discountedPrice) / room.Price * 100; // 20% discount
        roomRepositoryMock.Setup(x => x.GetRoomAsync(roomId)).ReturnsAsync(room);

        // Act
        var result = await sut.AddDiscountAsync(roomId, command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(roomId, result.RoomId);
        Assert.Equal(discountedPrice, result.DiscountedPrice);
        Assert.Equal(room.Price, result.OriginalPrice);
        Assert.Equal(expectedPercentage, result.Percentage);
        roomRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddDiscountAsync_ShouldReturnCorrectDiscountData_WhenPercentageProvided()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var room = fixture.Build<Room>()
            .With(x => x.Id, roomId)
            .With(x => x.Price, 100m)
            .Create();

        var percentage = 20m;


        var command = GetValidCreateDiscountCommand();
        command.Percentage = percentage;
        command.DiscountedPrice = null;

        var expectedDiscountedPrice = room.Price - room.Price * percentage / 100; // 80
        var expectedRoundedDiscountedPrice = Math.Round(expectedDiscountedPrice, 2); // 80.00
        roomRepositoryMock.Setup(x => x.GetRoomAsync(roomId)).ReturnsAsync(room);

        // Act
        var result = await sut.AddDiscountAsync(roomId, command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(roomId, result.RoomId);
        Assert.Equal(percentage, result.Percentage);
        Assert.Equal(room.Price, result.OriginalPrice);
        Assert.Equal(expectedRoundedDiscountedPrice, result.DiscountedPrice);
        roomRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddDiscountAsync_ShouldThrowBadRequestException_WhenNoDiscountDataProvided()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var command = GetValidCreateDiscountCommand();

        command.DiscountedPrice = null;
        command.Percentage = null;

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => sut.AddDiscountAsync(roomId, command));
    }

    [Fact]
    public async Task AddDiscountAsync_ShouldThrowNotFoundException_WhenRoomDoesNotExist()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var command = GetValidCreateDiscountCommand();
        roomRepositoryMock.Setup(x => x.GetRoomAsync(roomId)).ReturnsAsync((Room?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.AddDiscountAsync(roomId, command));
    }

    [Theory]
    [InlineData(100, 80, 20)] // 20% discount
    [InlineData(100, 20, 80)] // 80% discount
    [InlineData(200, 150, 25)] // 25% discount
    [InlineData(150, 120, 20)] // 20% discount
    public void DiscountConstructor_ShouldSetCorrectPercentage(decimal originalPrice, decimal discountedPrice, decimal expectedPercentage)
    {
        // Arrange
        var room = new Room { Price = originalPrice };
        var startDate = DateTime.UtcNow.AddDays(1);
        var endDate = DateTime.UtcNow.AddDays(10);

        // Act
        var discount = new Discount(room, originalPrice, discountedPrice, startDate, endDate);

        // Assert
        Assert.Equal(expectedPercentage, discount.Percentage);
    }


    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public void DiscountConstructor_ShouldThrowArgumentException_WhenPercentageIsOutOfRange(
        decimal percentage)
    {
        // Arrange
        var room = new Room { Price = 100m };
        var startDate = DateTime.UtcNow.AddDays(1);
        var endDate = DateTime.UtcNow.AddDays(10);
        decimal invalidPercentage = percentage; // Percentage outside valid range (1-100)

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Discount(room, invalidPercentage, startDate, endDate));
        Assert.Contains("Percentage must be between 1 and 100", ex.Message);
    }

    [Fact]
    public void DiscountConstructor_ShouldThrowArgumentException_WhenStartDateIsInThePast()
    {
        // Arrange
        var room = new Room { Price = 100m };
        var startDate = DateTime.UtcNow.AddDays(-1); // Start date in the past
        var endDate = DateTime.UtcNow.AddDays(10);
        decimal percentage = 20m;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Discount(room, percentage, startDate, endDate));
        Assert.Contains("Start and End dates must be in the future", ex.Message);
    }

    [Fact]
    public void DiscountConstructor_ShouldThrowArgumentException_WhenEndDateIsInThePast()
    {
        // Arrange
        var room = new Room { Price = 100m };
        var startDate = DateTime.UtcNow.AddDays(1); // End date in the past
        var endDate = DateTime.UtcNow.AddDays(-1);
        decimal percentage = 20m;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Discount(room, percentage, startDate, endDate));
        Assert.Contains("Start and End dates must be in the future", ex.Message);
    }

    [Fact]
    public void DiscountConstructor_ShouldThrowArgumentException_WhenEndDateIsBeforeStartDate()
    {
        // Arrange
        var room = new Room { Price = 100m };
        var startDate = DateTime.UtcNow.AddDays(10);
        var endDate = DateTime.UtcNow.AddDays(1); // End date before start date
        decimal percentage = 20m;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Discount(room, percentage, startDate, endDate));
        Assert.Contains("Start date must be before end date", ex.Message);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(101)]
    public void DiscountConstructor_ShouldThrowArgumentException_WhenDiscountedPriceIsNotLessThanOriginalPrice(
        decimal invalidDiscountedPrice)
    {
        // Arrange
        var room = new Room { Price = 100m };
        var startDate = DateTime.UtcNow.AddDays(1);
        var endDate = DateTime.UtcNow.AddDays(10);
        decimal originalPrice = 100m;
        decimal discountedPrice = invalidDiscountedPrice; // Not less than original price

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Discount(room, originalPrice, discountedPrice, startDate, endDate));
        Assert.Contains("Discounted price must be less than original price", ex.Message);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(-1, -1)]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    public void DiscountConstructor_ShouldThrowArgumentException_WhenOriginalPriceOrDiscountedPriceIsZeroOrLess(
        decimal invalidOriginalPrice, decimal invalidDiscountedPrice)
    {
        // Arrange
        var room = new Room { Price = 100m };
        var startDate = DateTime.UtcNow.AddDays(1);
        var endDate = DateTime.UtcNow.AddDays(10);
        decimal originalPrice = invalidOriginalPrice; 
        decimal discountedPrice = invalidDiscountedPrice;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Discount(room, originalPrice, discountedPrice, startDate, endDate));
        Assert.Contains("Prices must be greater than 0", ex.Message);
    }

}
