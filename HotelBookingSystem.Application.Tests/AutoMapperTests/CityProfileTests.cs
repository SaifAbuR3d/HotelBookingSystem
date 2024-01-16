using HotelBookingSystem.Application.DTOs.City.Command;
using HotelBookingSystem.Application.DTOs.City.OutputModel;

namespace HotelBookingSystem.Application.Tests.AutoMapperTests;

public class CityProfileTests
{
    private readonly IMapper mapper;
    private readonly IFixture fixture;

    public CityProfileTests()
    {
        fixture = FixtureFactory.CreateFixture();
        mapper = AutoMapperSingleton.Mapper;
    }

    [Fact]
    public void CreateCityCommandToCityMapping()
    {
        // Arrange
        var createCityCommand = fixture.Create<CreateCityCommand>();

        // Act
        var city = mapper.Map<City>(createCityCommand);

        // Assert
        Assert.NotNull(city);
        Assert.IsType<City>(city);
        Assert.Equal(createCityCommand.Name, city.Name);
        Assert.Equal(createCityCommand.Country, city.Country);
        Assert.Equal(createCityCommand.PostOffice, city.PostOffice);
    }

    [Fact]
    public void CityToCityOutputModelMapping()
    {
        // Arrange
        var city = fixture.Create<City>();
        city.Hotels = fixture.CreateMany<Hotel>(5).ToList();

        // Act
        var cityOutputModel = mapper.Map<CityOutputModel>(city);

        // Assert
        Assert.NotNull(cityOutputModel);
        Assert.IsType<CityOutputModel>(cityOutputModel);
        Assert.Equal(city.Id, cityOutputModel.Id);
        Assert.Equal(city.Name, cityOutputModel.Name);
        Assert.Equal(city.Country, cityOutputModel.Country);
        Assert.Equal(city.PostOffice, cityOutputModel.PostOffice);
        Assert.Equal(city.Hotels.Count, cityOutputModel.NumberOfHotels);
    }

    [Fact]
    public void UpdateCityCommandToCityMapping()
    {
        // Arrange
        var updateCityCommand = fixture.Create<UpdateCityCommand>();

        // Act
        var city = mapper.Map<City>(updateCityCommand);

        // Assert
        Assert.NotNull(city);
        Assert.IsType<City>(city);
        Assert.Equal(updateCityCommand.Name, city.Name);
        Assert.Equal(updateCityCommand.Country, city.Country);
        Assert.Equal(updateCityCommand.PostOffice, city.PostOffice);
    }

    [Fact]
    public void CityImageToCityImageOutputModelMapping()
    {
        // Arrange
        var cityImage = fixture.Create<CityImage>();

        // Act
        var cityImageOutputModel = mapper.Map<CityImageOutputModel>(cityImage);

        // Assert
        Assert.NotNull(cityImageOutputModel);
        Assert.Equal(cityImage.ImageUrl, cityImageOutputModel.ImageUrl);
        Assert.Equal(cityImage.AlternativeText, cityImageOutputModel.AlternativeText);
    }


    [Fact]
    public void CityToCityAsTrendingDestinationOutputModelMapping()
    {
        // Arrange
        var city = fixture.Create<City>();
        city.Images = fixture.CreateMany<CityImage>(3).ToList();

        // Act
        var cityAsTrendingDestinationOutputModel = mapper.Map<CityAsTrendingDestinationOutputModel>(city);

        // Assert
        Assert.NotNull(cityAsTrendingDestinationOutputModel);
        Assert.IsType<CityAsTrendingDestinationOutputModel>(cityAsTrendingDestinationOutputModel);
        Assert.Equal(city.Id, cityAsTrendingDestinationOutputModel.Id);
        Assert.Equal(city.Name, cityAsTrendingDestinationOutputModel.Name);
        Assert.Equal(city.Country, cityAsTrendingDestinationOutputModel.Country);
        Assert.NotNull(cityAsTrendingDestinationOutputModel.CityImage);
        Assert.IsType<CityImageOutputModel>(cityAsTrendingDestinationOutputModel.CityImage);
    }



}
