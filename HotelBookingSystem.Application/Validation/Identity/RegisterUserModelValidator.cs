using FluentValidation;
using HotelBookingSystem.Application.DTOs.Identity.Command;
using HotelBookingSystem.Application.Validation.Common;

namespace HotelBookingSystem.Application.Validation.Identity;

public class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
{
    public RegisterUserModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(5).WithMessage("Username must be at least 5 characters long")
            .MaximumLength(30).WithMessage("Username must not be longer than 30 characters");

        RuleFor(x => x.Password)
            .StrongPassword(); // Custom extension method

        RuleFor(x => x.FirstName)
            .ValidName(2, 30); // Custom extension method

        RuleFor(x => x.LastName)
            .ValidName(2, 30);
    }
}
