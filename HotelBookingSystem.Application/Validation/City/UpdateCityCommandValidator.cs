using FluentValidation;
using HotelBookingSystem.Application.DTOs.City.Command;
using HotelBookingSystem.Application.Validation.Common;

namespace HotelBookingSystem.Application.Validation.City;

using static HotelBookingSystem.Domain.Constants.Common;
using static HotelBookingSystem.Domain.Constants.City;

public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
{
    public UpdateCityCommandValidator()
    {
        RuleFor(c => c.Name)
            .ValidName(MinNameLength, MaxNameLength);

        RuleFor(c => c.Country)
            .ValidName(MinNameLength, MaxNameLength);

        RuleFor(c => c.PostOffice)
            .ValidNumericString(PostOfficeLength);
    }
}
