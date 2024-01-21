using FluentValidation;
using HotelBookingSystem.Application.DTOs.City.Command;
using HotelBookingSystem.Application.Validation.Common;

namespace HotelBookingSystem.Application.Validation.City;

using static HotelBookingSystem.Domain.Constants.Common;
using static HotelBookingSystem.Domain.Constants.City;

public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
{
    public CreateCityCommandValidator()
    {
        RuleFor(c => c.Name)
            .ValidName(MinNameLength, MaxNameLength);

        RuleFor(c => c.Country)
            .ValidName(MinNameLength, MaxNameLength);

        RuleFor(c => c.PostOffice)
            .ValidNumericString(PostOfficeLength);
    }

}
