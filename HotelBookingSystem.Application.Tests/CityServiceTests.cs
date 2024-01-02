using HotelBookingSystem.Application.Abstractions.RepositoryInterfaces;
using HotelBookingSystem.Application.Abstractions.ServiceInterfaces;
using HotelBookingSystem.Application.DTOs.City.Command;
using HotelBookingSystem.Application.DTOs.City.OutputModel;
using HotelBookingSystem.Application.DTOs.City.Query;
using HotelBookingSystem.Application.DTOs.Common;
using HotelBookingSystem.Application.Tests.Shared;

namespace HotelBookingSystem.Application.Tests;

public class CityServiceTests
{
    private readonly Mock<ICityRepository> cityRepositoryMock;
    private readonly IFixture fixture;
    private readonly CityService sut;
    private readonly IMapper mapper;

    public CityServiceTests()
    {
        fixture = FixtureFactory.CreateFixture();
        cityRepositoryMock = new Mock<ICityRepository>();
        mapper = AutoMapperSingleton.Mapper;
        var imageHandler = new Mock<IImageHandler>();

        sut = new CityService(cityRepositoryMock.Object,
                              mapper,
                              imageHandler.Object);
    }

    [Fact]
    public async Task GetAllCitiesAsync_ShouldReturnAllCitiesWithCorrectPaginationMetadata_IfCitiesCountIsLessThanOrEqualPageSize()
    {
        // Arrange
        var expectedCities = fixture.CreateMany<City>(10);
        var parameters = new GetCitiesQueryParameters();
        var expectedPaginationMetadata = new PaginationMetadata(1, 10, 10); //page 1, 10 items per page, 10 total items

        cityRepositoryMock.Setup(x => x.GetAllCitiesAsync(parameters)).ReturnsAsync((expectedCities, expectedPaginationMetadata));

        // Act
        var (cities, paginationMetadata) = await sut.GetAllCitiesAsync(parameters);

        // Assert
        cityRepositoryMock.Verify(c => c.GetAllCitiesAsync(parameters), Times.Once);
        Assert.IsAssignableFrom<IEnumerable<CityOutputModel>>(cities);
        Assert.Equal(expectedCities.Count(), cities.Count());

        Assert.IsType<PaginationMetadata>(paginationMetadata);
        Assert.Equal(expectedPaginationMetadata.PageNumber, paginationMetadata.PageNumber);
        Assert.Equal(expectedPaginationMetadata.PageSize, paginationMetadata.PageSize);
        Assert.Equal(expectedPaginationMetadata.TotalCount, paginationMetadata.TotalCount);
        Assert.False(paginationMetadata.HasPreviousPage);
        Assert.False(paginationMetadata.HasNextPage);
    }

    [Fact]
    public async Task GetCityAsync_ShouldReturnCity_IfCityExists()
    {
        // Arrange
        var city = fixture.Create<City>();
        cityRepositoryMock.Setup(x => x.GetCityAsync(city.Id)).ReturnsAsync(city);

        // Act
        var result = await sut.GetCityAsync(city.Id);

        // Assert
        cityRepositoryMock.Verify(c => c.GetCityAsync(city.Id), Times.Once);
        Assert.NotNull(result);
        Assert.IsType<CityOutputModel>(result);
        Assert.Equal(city.Name, result.Name);
    }
    [Fact]
    public async Task GetCityAsync_ShouldThrowNotFoundException_IfCityDoesNotExist()
    {
        // Arrange
        cityRepositoryMock.Setup(x => x.GetCityAsync(It.IsAny<Guid>())).ReturnsAsync((City?)null);

        // Act & Assert
        var result = await Assert.ThrowsAsync<NotFoundException>(() => sut.GetCityAsync(It.IsAny<Guid>()));
        cityRepositoryMock.Verify(c => c.GetCityAsync(It.IsAny<Guid>()), Times.Once);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteCityAsync_ShouldCallSaveChangesAsyncAndReturnTrue_IfCityExists()
    {
        // Arrange
        cityRepositoryMock.Setup(x => x.DeleteCityAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        // Act
        var result = await sut.DeleteCityAsync(It.IsAny<Guid>());

        // Assert
        cityRepositoryMock.Verify(c => c.DeleteCityAsync(It.IsAny<Guid>()), Times.Once);
        cityRepositoryMock.Verify(c => c.SaveChangesAsync(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteCityAsync_ShouldNotCallSaveChangesAsyncAndReturnFalse_IfCityDoesNotExist()
    {
        // Arrange
        cityRepositoryMock.Setup(x => x.DeleteCityAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        // Act
        var result = await sut.DeleteCityAsync(It.IsAny<Guid>());

        // Assert
        cityRepositoryMock.Verify(c => c.DeleteCityAsync(It.IsAny<Guid>()), Times.Once);
        cityRepositoryMock.Verify(c => c.SaveChangesAsync(), Times.Never);
        Assert.False(result);

    }

    [Fact]
    public async Task CreateCityAsync_ShouldReturnCreatedCity()
    {
        // Arrange
        var createCityCommand = fixture.Create<CreateCityCommand>();
        var city = mapper.Map<City>(createCityCommand);

        cityRepositoryMock.Setup(x => x.AddCityAsync(It.IsAny<City>())).ReturnsAsync(city);

        // Act
        var result = await sut.CreateCityAsync(createCityCommand);

        // Assert
        cityRepositoryMock.Verify(c => c.AddCityAsync(It.IsAny<City>()), Times.Once);
        cityRepositoryMock.Verify(c => c.SaveChangesAsync(), Times.Once);
        Assert.Equal(city.Name, result.Name);
    }

    [Fact]
    public async Task UpdateCityAsync_ShouldCallSaveChangesAndReturnTrue_IfCityExists()
    {
        // Arrange
        var updateCityCommand = fixture.Create<UpdateCityCommand>();
        var city = mapper.Map<City>(updateCityCommand);
        cityRepositoryMock.Setup(x => x.GetCityAsync(It.IsAny<Guid>())).ReturnsAsync(city);

        // Act
        var result = await sut.UpdateCityAsync(It.IsAny<Guid>(), updateCityCommand);

        // Assert
        cityRepositoryMock.Verify(c => c.GetCityAsync(It.IsAny<Guid>()), Times.Once);
        cityRepositoryMock.Verify(c => c.SaveChangesAsync(), Times.Once);
        Assert.True(result);
    }
    [Fact]
    public async Task UpdateCityAsync_ShouldThrowNotFoundException_IfCityDoesNotExist()
    {
        // Arrange
        cityRepositoryMock.Setup(x => x.GetCityAsync(It.IsAny<Guid>())).ReturnsAsync((City?)null);

        // Act & Asser
        var result = await Assert.ThrowsAsync<NotFoundException>(() => sut.UpdateCityAsync(It.IsAny<Guid>(), It.IsAny<UpdateCityCommand>()));
        Assert.NotNull(result);
    }
    [Fact]
    public async Task CityExistsAsync_ShouldCallRepositoryCityExistsAsync()
    {
        // Act
        var result = await sut.CityExistsAsync(It.IsAny<Guid>());

        // Assert
        cityRepositoryMock.Verify(c => c.CityExistsAsync(It.IsAny<Guid>()), Times.Once);
    }
}