using HotelBookingSystem.Application.DTOs.Discount;
using HotelBookingSystem.Application.Mapping;

namespace HotelBookingSystem.Application.Tests.AutoMapperTests;

public class DiscountProfileTests
{
    private readonly IMapper mapper;
    private readonly IFixture fixture;

    public DiscountProfileTests()
    {
        fixture = FixtureFactory.CreateFixture();
        mapper = AutoMapperSingleton.Mapper;
    }

    [Fact]
    public void DiscountToDiscountOutputModelMapping()
    {
        // Arrange
        var discount = fixture.Build<Discount>()
                              .With(d => d.Room, fixture.Create<Room>())
                              .Create();

        // Act
        var discountOutputModel = mapper.Map<DiscountOutputModel>(discount);

        // Assert
        Assert.NotNull(discountOutputModel);
        Assert.IsType<DiscountOutputModel>(discountOutputModel);
        Assert.Equal(discount.Id, discountOutputModel.Id);
        Assert.Equal(discount.RoomId, discountOutputModel.RoomId);
        Assert.Equal(discount.Percentage, discountOutputModel.Percentage);
        Assert.Equal(discount.StartDate, discountOutputModel.StartDate);
        Assert.Equal(discount.EndDate, discountOutputModel.EndDate);

        // OriginalPrice and DiscountedPrice are ignored in the mapping,
        // because they are calculated at The service
        Assert.Equal(0, discountOutputModel.OriginalPrice);
        Assert.Equal(0, discountOutputModel.DiscountedPrice);
    }
}