using FluentValidation;
using HotelBookingSystem.Application.DTOs.Hotel.Command;
using HotelBookingSystem.Application.Validation.Common;

namespace HotelBookingSystem.Application.Validation.Hotel;

using static Domain.Models.Constants.Common;
using static Domain.Models.Constants.Hotel;

public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(h => h.Name)
            .ValidName(MinNameLength, MaxNameLength);

        RuleFor(h => h.Owner)
            .ValidName(MinNameLength, MaxNameLength);

        RuleFor(h => h.CityName)
            .ValidName(MinNameLength, MaxNameLength);

        RuleFor(h => h.StarRate)
            .InclusiveBetween(MinHotelStars, MaxHotelStars);

        RuleFor(h => h.Location) ////
            .NotEmpty();
    }
}
