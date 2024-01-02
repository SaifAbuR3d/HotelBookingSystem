using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.DTOs.Hotel.Command;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;
using HotelBookingSystem.Application.DTOs.Hotel.Query;
using HotelBookingSystem.Application.Tests.Shared;

namespace HotelBookingSystem.Application.Tests;

public class HotelServiceTests
{
    private readonly Mock<ICityRepository> cityRepositoryMock;
    private readonly Mock<IHotelRepository> hotelRepositoryMock;
    private readonly Mock<IGuestRepository> guestRepositoryMock; 
    private readonly IFixture fixture;
    private readonly IMapper mapper;
    private readonly HotelService sut;

    public HotelServiceTests()
    {
        fixture = FixtureFactory.CreateFixture();
        cityRepositoryMock = new Mock<ICityRepository>();
        hotelRepositoryMock = new Mock<IHotelRepository>();
        guestRepositoryMock = new Mock<IGuestRepository>();
        mapper = AutoMapperSingleton.Mapper;
        var imageHandlerMock = new Mock<IImageHandler>();

        sut = new HotelService(hotelRepositoryMock.Object,
                               cityRepositoryMock.Object,
                               guestRepositoryMock.Object,
                               mapper,
                               imageHandlerMock.Object);
    }
    [Fact]
    public async Task GetAllHotelsAsync_ShouldReturnAllHotelsWithCorrectPaginationMetadata_IfHotelsCountIsLessThanOrEqualPageSize()
    {
        // Arrange
        var expectedHotels = fixture.CreateMany<Hotel>(10);
        var parameters = new GetHotelsQueryParameters();
        var expectedPaginationMetadata = new PaginationMetadata(1, 10, 10); //page 1, 10 items per page, 10 total items

        hotelRepositoryMock.Setup(x => x.GetAllHotelsAsync(parameters)).ReturnsAsync((expectedHotels, expectedPaginationMetadata));

        // Act
        var (hotels, paginationMetadata) = await sut.GetAllHotelsAsync(parameters);

        // Assert
        hotelRepositoryMock.Verify(h => h.GetAllHotelsAsync(parameters), Times.Once);
        Assert.IsAssignableFrom<IEnumerable<HotelOutputModel>>(hotels);
        Assert.Equal(expectedHotels.Count(), hotels.Count());

        Assert.IsType<PaginationMetadata>(paginationMetadata);
        Assert.Equal(expectedPaginationMetadata.PageNumber, paginationMetadata.PageNumber);
        Assert.Equal(expectedPaginationMetadata.PageSize, paginationMetadata.PageSize);
        Assert.Equal(expectedPaginationMetadata.TotalCount, paginationMetadata.TotalCount);
        Assert.False(paginationMetadata.HasPreviousPage);
        Assert.False(paginationMetadata.HasNextPage);
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
