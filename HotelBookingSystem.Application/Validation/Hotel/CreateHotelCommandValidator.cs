﻿using FluentValidation;
using HotelBookingSystem.Application.DTOs.Hotel.Command;
using HotelBookingSystem.Application.Validation.Common;

namespace HotelBookingSystem.Application.Validation.Hotel;

using static Domain.Models.Constants.Common;
using static Domain.Models.Constants.Hotel;
using static Domain.Models.Constants.Location;

public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(h => h.Name)
            .ValidName(MinNameLength, MaxNameLength);

        RuleFor(h => h.Owner)
            .ValidName(MinNameLength, MaxNameLength);

        RuleFor(h => h.StarRate)
            .InclusiveBetween(MinHotelStars, MaxHotelStars);

        RuleFor(h => h.Street) 
            .NotEmpty();
        
        RuleFor(h => h.Latitude)
            .NotEmpty()
            .InclusiveBetween(MinLatitude, MaxLatitude);

        RuleFor(h => h.Longitude)
            .NotEmpty()
            .InclusiveBetween(MinLongitude, MaxLongitude);
    }
}
