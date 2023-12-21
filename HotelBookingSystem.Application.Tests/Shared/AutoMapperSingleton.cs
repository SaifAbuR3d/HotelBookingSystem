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
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new CityProfile());
                    mc.AddProfile(new HotelProfile());
                    mc.AddProfile(new RoomProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            return _mapper;
        }
    }
}