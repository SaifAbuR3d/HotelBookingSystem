using HotelBookingSystem.Application.DTOs.Guest;

namespace HotelBookingSystem.Application.Tests.AutoMapperTests;

public class GuestProfileTests
{
    private readonly IMapper mapper;
    private readonly IFixture fixture;

    public GuestProfileTests()
    {
        fixture = FixtureFactory.CreateFixture();
        mapper = AutoMapperSingleton.Mapper;
    }

    [Fact]
    public void GuestToGuestOutputModelMappingTest()
    {
        // Arrange
        var guest = fixture.Create<Guest>();

        // Act
        var guestOutputModel = mapper.Map<GuestOutputModel>(guest);

        // Assert
        Assert.Equal(guest.Id, guestOutputModel.Id);
        Assert.Equal(guest.FirstName, guestOutputModel.FirstName);
        Assert.Equal(guest.LastName, guestOutputModel.LastName);
        Assert.Equal(guest.FullName, guestOutputModel.FullName);
        Assert.Equal(guest.Bookings.Count, guestOutputModel.NumberOfBookings);
    }
}
