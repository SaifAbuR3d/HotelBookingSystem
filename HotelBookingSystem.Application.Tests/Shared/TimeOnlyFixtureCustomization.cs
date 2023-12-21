namespace HotelBookingSystem.Application.Tests.Shared;

public class TimeOnlyFixtureCustomization : ICustomization
{

    void ICustomization.Customize(IFixture fixture)
    {
        fixture.Customize<TimeOnly>(composer => composer.FromFactory<DateTime>(TimeOnly.FromDateTime));
    }
}
