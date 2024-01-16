using HotelBookingSystem.Application.DTOs.Hotel.OutputModel;

namespace HotelBookingSystem.Application.Tests.AutoMapperTests;

public class FeaturedDealProfileTests
{
    private readonly IMapper mapper;
    private readonly IFixture fixture;

    public FeaturedDealProfileTests()
    {
        fixture = FixtureFactory.CreateFixture();
        mapper = AutoMapperSingleton.Mapper;
    }

    [Fact]
    public void RoomToFeaturedDealOutputModelMapping()
    {
        // Arrange
        var discount = fixture.Create<Discount>();
        var hotel = fixture.Create<Hotel>();
        var room = fixture.Build<Room>()
                          .With(r => r.Hotel, hotel)
                          .With(r => r.Discounts, [discount])
                          .Create();

        // Act
        var featuredDeal = mapper.Map<FeaturedDealOutputModel>(room);

        // Assert
        Assert.NotNull(featuredDeal);
        Assert.IsType<FeaturedDealOutputModel>(featuredDeal);
        Assert.Equal(room.Hotel.Id, featuredDeal.HotelId);
        Assert.Equal(room.Hotel.Name, featuredDeal.HotelName);
        Assert.Equal(room.Hotel.StarRate, featuredDeal.StarRate);
        Assert.Equal(room.Hotel.Latitude, featuredDeal.Latitude);
        Assert.Equal(room.Hotel.Longitude, featuredDeal.Longitude);
        Assert.Equal(room.Hotel.Street, featuredDeal.Street);
        Assert.Equal(room.Hotel.City.Name, featuredDeal.CityName);
        Assert.Equal(room.Hotel.City.Country, featuredDeal.Country);
        Assert.Equal(room.Id, featuredDeal.RoomId);
        Assert.Equal(room.Price, featuredDeal.OriginalPrice);
        Assert.NotNull(featuredDeal.HotelImage); 
        Assert.IsType<HotelImageOutputModel>(featuredDeal.HotelImage);
        Assert.Equal(room.Discounts.First().DiscountedPrice, featuredDeal.DiscountedPrice);
        Assert.Equal(room.Discounts.First().Percentage, (decimal)featuredDeal.DiscountPercentage);
    }
}