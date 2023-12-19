using FluentValidation;
using HotelBookingSystem.Application.DTOs.City;

namespace HotelBookingSystem.Application.Validation.City;

using static Domain.Models.Constants.Common;
using static Domain.Models.Constants.City;

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
