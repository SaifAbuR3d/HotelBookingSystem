using HotelBookingSystem.Application.DTOs.City;
using HotelBookingSystem.Application.DTOs.Hotel;
using HotelBookingSystem.Application.Tests.Shared;

namespace HotelBookingSystem.Application.Tests;

public class HotelServiceTests
{
    private readonly Mock<ICityRepository> cityRepositoryMock;
    private readonly Mock<IHotelRepository> hotelRepositoryMock;
    private readonly IFixture fixture;
    private readonly IMapper mapper;
    private readonly HotelService sut;

    public HotelServiceTests()
    {
        fixture = FixtureFactory.CreateFixture();
        cityRepositoryMock = new Mock<ICityRepository>();
        hotelRepositoryMock = new Mock<IHotelRepository>();
        mapper = AutoMapperSingleton.Mapper;
        sut = new HotelService(hotelRepositoryMock.Object,cityRepositoryMock.Object, mapper);
    }
    [Fact]
    public async Task GetAllHotelsAsync_ShouldReturnAllHotels()
    {
        // Arrange
        var hotels = fixture.CreateMany<Hotel>(10);
        hotelRepositoryMock.Setup(x => x.GetAllHotelsAsync()).ReturnsAsync(hotels);

        // Act
        var result = await sut.GetAllHotelsAsync();

        // Assert
        hotelRepositoryMock.Verify(h => h.GetAllHotelsAsync(), Times.Once);
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<HotelOutputModel>>(result);
        Assert.Equal(hotels.Count(), result.Count());
    }
    [Fact]
    public async Task GetHotelAsync_ShouldReturnHotel_IfHotelExists()
    {
        // Arrange
        var hotel = fixture.Create<Hotel>();
        hotelRepositoryMock.Setup(x => x.GetHotelAsync(hotel.Id)).ReturnsAsync(hotel);

        // Act
        var result = await sut.GetHotelAsync(hotel.Id);

        // Assert
        hotelRepositoryMock.Verify(h => h.GetHotelAsync(hotel.Id), Times.Once);
        Assert.NotNull(result);
        Assert.IsType<HotelOutputModel>(result);
        Assert.Equal(hotel.Name, result.Name);
    }
    [Fact]
    public async Task GetHotelAsync_ShouldThrowNotFoundException_IfHotelDoesNotExist()
    {
        // Arrange
        hotelRepositoryMock.Setup(x => x.GetHotelAsync(It.IsAny<Guid>())).ReturnsAsync((Hotel?)null);

        // Act & Assert
        var result = await Assert.ThrowsAsync<NotFoundException>(() => sut.GetHotelAsync(Guid.NewGuid()));
        hotelRepositoryMock.Verify(h => h.GetHotelAsync(It.IsAny<Guid>()), Times.Once); 
        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteHotelAsync_ShouldCallSaveChangesAndReturnTrue_IfHotelExists()
    {
        // Arrange
        hotelRepositoryMock.Setup(x => x.DeleteHotelAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        // Act
        var result = await sut.DeleteHotelAsync(It.IsAny<Guid>());

        // Assert
        hotelRepositoryMock.Verify(h => h.DeleteHotelAsync(It.IsAny<Guid>()), Times.Once);
        hotelRepositoryMock.Verify(c => c.SaveChangesAsync(), Times.Once);
        Assert.True(result);
    }
    [Fact]
    public async Task DeleteHotelAsync_ShouldNotCallSaveChangesAndReturnFalse_IfHotelDoesNotExist()
    {
        // Arrange
        hotelRepositoryMock.Setup(x => x.DeleteHotelAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        // Act
        var result = await sut.DeleteHotelAsync(It.IsAny<Guid>());

        // Assert
        hotelRepositoryMock.Verify(h => h.DeleteHotelAsync(It.IsAny<Guid>()), Times.Once);
        hotelRepositoryMock.Verify(c => c.SaveChangesAsync(), Times.Never);
        Assert.False(result);
    }
    [Fact]
    public async Task CreateHotelAsync_ShouldReturnCreatedHotel_IfCityNameExists()
    {
        // Arrange
        var createHotelCommand = fixture.Create<CreateHotelCommand>();
        var city = fixture.Create<City>();
        cityRepositoryMock.Setup(x => x.GetCityByNameAsync(createHotelCommand.CityName)).ReturnsAsync(city);
        var hotel = mapper.Map<Hotel>(createHotelCommand);
        hotelRepositoryMock.Setup(x => x.AddHotelAsync(It.IsAny<Hotel>())).ReturnsAsync(hotel);

        // Act
        var result = await sut.CreateHotelAsync(createHotelCommand);

        // Assert
        cityRepositoryMock.Verify(c => c.GetCityByNameAsync(createHotelCommand.CityName), Times.Once);
        hotelRepositoryMock.Verify(h => h.AddHotelAsync(It.IsAny<Hotel>()), Times.Once);
        hotelRepositoryMock.Verify(h => h.SaveChangesAsync(), Times.Once);
        Assert.NotNull(result);
        Assert.IsType<HotelOutputModel>(result);
        Assert.Equal(createHotelCommand.Name, result.Name);
    }

    [Fact]
    public async Task CreateHotelAsync_ShouldThrowNotFoundException_IfCityNameDoesNotExist()
    {
        // Arrange
        var createHotelCommand = fixture.Create<CreateHotelCommand>();
        cityRepositoryMock.Setup(x => x.GetCityByNameAsync(createHotelCommand.CityName)).ReturnsAsync((City?)null);

        // Act & Assert
        var result = await Assert.ThrowsAsync<NotFoundException>(() => sut.CreateHotelAsync(createHotelCommand));
        cityRepositoryMock.Verify(c => c.GetCityByNameAsync(createHotelCommand.CityName), Times.Once);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateHotelAsync_ShouldCallSaveChangesAndReturnTrue_IfHotelAndCityExist()
    {
        // Arrange
        var updateHotelCommand = fixture.Create<UpdateHotelCommand>();
        var hotel = mapper.Map<Hotel>(updateHotelCommand);
        hotelRepositoryMock.Setup(x => x.GetHotelAsync(It.IsAny<Guid>())).ReturnsAsync(hotel);
        var city = fixture.Create<City>();
        cityRepositoryMock.Setup(x => x.GetCityByNameAsync(updateHotelCommand.CityName)).ReturnsAsync(city);


        // Act
        var result = await sut.UpdateHotelAsync(It.IsAny<Guid>(), updateHotelCommand);

        // Assert
        hotelRepositoryMock.Verify(h => h.GetHotelAsync(It.IsAny<Guid>()), Times.Once);
        hotelRepositoryMock.Verify(h => h.SaveChangesAsync(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateHotelAsync_ShouldThrowNotFoundException_IfCityDoesNotExist()
    {
        // Arrange
        var updateHotelCommand = fixture.Create<UpdateHotelCommand>();
        cityRepositoryMock.Setup(x => x.GetCityByNameAsync(updateHotelCommand.CityName)).ReturnsAsync((City?)null);

        // Act & Asser
        var result = await Assert.ThrowsAsync<NotFoundException>(() => sut.UpdateHotelAsync(It.IsAny<Guid>(), updateHotelCommand));
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateHotelAsync_ShouldThrowNotFoundException_IfHotelDoesNotExist()
    {
        // Arrange
        hotelRepositoryMock.Setup(x => x.GetHotelAsync(It.IsAny<Guid>())).ReturnsAsync((Hotel?)null);
        var updateHotelCommand = fixture.Create<UpdateHotelCommand>();
        var city = fixture.Create<City>();
        cityRepositoryMock.Setup(x => x.GetCityByNameAsync(updateHotelCommand.CityName)).ReturnsAsync(city);

        // Act & Assert
        var result = await Assert.ThrowsAsync<NotFoundException>(() => sut.UpdateHotelAsync(It.IsAny<Guid>(), updateHotelCommand));
        Assert.NotNull(result);
    }

}
