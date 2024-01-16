using HotelBookingSystem.Application.Mapping;

namespace HotelBookingSystem.Application.Tests.Shared;

public class AutoMapperSingleton
{
    private static IMapper _mapper;
    public static IMapper Mapper
    {
        get
        {
            if (_mapper == null)
            {
                // Auto Mapper Configurations
                var mappingConfig = new MapperConfiguration(config =>
                {
                    config.AddProfile(new CityProfile());
                    config.AddProfile(new HotelProfile());
                    config.AddProfile(new RoomProfile());
                    config.AddProfile(new BookingProfile());
                    config.AddProfile(new DiscountProfile());
                    config.AddProfile(new FeaturedDealProfile());
                    config.AddProfile(new ReviewProfile());
                    config.AddProfile(new GuestProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            return _mapper;
        }
    }
}