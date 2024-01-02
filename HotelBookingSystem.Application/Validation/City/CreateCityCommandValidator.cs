using FluentValidation;
using HotelBookingSystem.Application.DTOs.City.Command;

namespace HotelBookingSystem.Application.Validation.City;

using static Domain.Models.Constants.Common;
using static Domain.Models.Constants.City;

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
