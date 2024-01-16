using HotelBookingSystem.Application.DTOs.Hotel.Command;
using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;

namespace HotelBookingSystem.Application.Tests.AutoMapperTests;

public class HotelProfileTests
{
    private readonly IMapper mapper;
    private readonly IFixture fixture;

    public HotelProfileTests()
    {
        fixture = FixtureFactory.CreateFixture();
        mapper = AutoMapperSingleton.Mapper;
    }

    [Fact]
    public void HotelToHotelOutputModelMappingTest()
    {
        // Arrange
        var hotel = fixture.Create<Hotel>();

        // Act
        var hotelOutputModel = mapper.Map<HotelOutputModel>(hotel);

        // Assert
        Assert.NotNull(hotelOutputModel);
        Assert.IsType<HotelOutputModel>(hotelOutputModel);
        Assert.Equal(hotel.Id, hotelOutputModel.Id);
        Assert.Equal(hotel.Name, hotelOutputModel.Name);
        Assert.Equal(hotel.Rooms.Count, hotelOutputModel.RoomsNumber);
    }

    [Fact]
    public void HotelImageToHotelImageOutputModelMappingTest()
    {
        // Arrange
        var hotelImage = fixture.Create<HotelImage>();

        // Act
        var hotelImageOutputModel = mapper.Map<HotelImageOutputModel>(hotelImage);

        // Assert
        Assert.NotNull(hotelImageOutputModel);
        Assert.IsType<HotelImageOutputModel>(hotelImageOutputModel);
        Assert.Equal(hotelImage.ImageUrl, hotelImageOutputModel.ImageUrl);
        Assert.Equal(hotelImage.AlternativeText, hotelImageOutputModel.AlternativeText);
    }

    [Fact]
    public void CreateHotelCommandToHotelMappingTest()
    {
        // Arrange
        var createHotelCommand = fixture.Create<CreateHotelCommand>();

        // Act
        var hotel = mapper.Map<Hotel>(createHotelCommand);

        // Assert
        Assert.NotNull(hotel);
        Assert.IsType<Hotel>(hotel);
        Assert.Equal(createHotelCommand.Name, hotel.Name);
        Assert.Equal(createHotelCommand.Owner, hotel.Owner);
    }

    [Fact]
    public void UpdateHotelCommandToHotelMappingTest()
    {
        // Arrange
        var updateHotelCommand = fixture.Create<UpdateHotelCommand>();
        var hotel = fixture.Create<Hotel>();

        // Act
        mapper.Map(updateHotelCommand, hotel);

        // Assert
        Assert.NotNull(hotel);
        Assert.IsType<Hotel>(hotel);
        Assert.Equal(updateHotelCommand.Name, hotel.Name);
        Assert.Equal(updateHotelCommand.Owner, hotel.Owner);
    }

    [Fact]
    public void BookingToRecentlyVisitedHotelOutputModelMappingTest()
    {
        // Arrange
        var booking = fixture.Create<Booking>();

        // Act
        var recentlyVisitedHotelOutputModel = mapper.Map<RecentlyVisitedHotelOutputModel>(booking);

        // Assert
        Assert.NotNull(recentlyVisitedHotelOutputModel);
        Assert.IsType<RecentlyVisitedHotelOutputModel>(recentlyVisitedHotelOutputModel);
        Assert.Equal(booking.Hotel.Name, recentlyVisitedHotelOutputModel.HotelName);
        Assert.Equal(booking.Hotel.City.Name, recentlyVisitedHotelOutputModel.CityName);
        Assert.Equal(booking.Hotel.StarRate, recentlyVisitedHotelOutputModel.StarRating);
        Assert.NotNull(recentlyVisitedHotelOutputModel.HotelImage);
        Assert.IsType<HotelImageOutputModel>(recentlyVisitedHotelOutputModel.HotelImage);
    }

    [Fact]
    public void HotelToHotelWithRoomsAndImagesOutputModelMappingTest()
    {
        // Arrange
        var hotel = fixture.Create<Hotel>();

        // Act
        var hotelWithRoomsAndImagesOutputModel = mapper.Map<HotelWithRoomsAndImagesOutputModel>(hotel);

        // Assert
        Assert.Equal(hotel.Name, hotelWithRoomsAndImagesOutputModel.Name);
        Assert.Equal(hotel.City.Name, hotelWithRoomsAndImagesOutputModel.CityName);
        Assert.Equal(hotel.Rooms.Select(r => r.Id), hotelWithRoomsAndImagesOutputModel.Rooms.Select(r => r.Id));
        Assert.Equal(hotel.Images.Select(i => i.ImageUrl), hotelWithRoomsAndImagesOutputModel.HotelImages.Select(i => i.ImageUrl));
    }

    [Fact]
    public void HotelToHotelSearchResultOutputModelMappingTest()
    {
        // Arrange
        var hotel = fixture.Create<Hotel>();

        // Act
        var hotelSearchResultOutputModel = mapper.Map<HotelSearchResultOutputModel>(hotel);

        // Assert
        Assert.Equal(hotel.Name, hotelSearchResultOutputModel.Name);
        Assert.Equal(hotel.StarRate, hotelSearchResultOutputModel.StarRate);
        Assert.Equal(hotel.Rooms.Min(r => r.Price), hotelSearchResultOutputModel.PriceStartingFrom);
        Assert.Equal(hotel.Description, hotelSearchResultOutputModel.Description);
        Assert.NotNull(hotelSearchResultOutputModel.HotelImage);
        Assert.IsType<HotelImageOutputModel>(hotelSearchResultOutputModel.HotelImage);
        Assert.Equal(hotel.Images.First().ImageUrl, hotelSearchResultOutputModel.HotelImage.ImageUrl);
    }

    [Fact]
    public void HotelToHotelWithinInvoiceMappingTest()
    {
        // Arrange
        var hotel = fixture.Create<Hotel>();

        // Act
        var hotelWithinInvoice = mapper.Map<HotelWithinInvoice>(hotel);

        // Assert
        Assert.Equal(hotel.Id, hotelWithinInvoice.Id);
        Assert.Equal(hotel.Name, hotelWithinInvoice.Name);
        Assert.Equal(hotel.City.Name, hotelWithinInvoice.CityName);
    }

}
