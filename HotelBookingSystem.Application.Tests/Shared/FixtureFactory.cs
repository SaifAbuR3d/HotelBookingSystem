namespace HotelBookingSystem.Application.Tests.Shared;

public static class FixtureFactory
{
    public static IFixture CreateFixture()
    {
        var fixture = new Fixture().Customize(new CompositeCustomization(
            new AutoMoqCustomization(),
            new DateOnlyFixtureCustomization(),
            new TimeOnlyFixtureCustomization())
            );

        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));

        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        return fixture;
    }
}