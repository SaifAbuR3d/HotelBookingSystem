using FluentValidation;
using HotelBookingSystem.Application.DTOs.Discount;

namespace HotelBookingSystem.Application.Validation.Discount;

public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
{
    public CreateDiscountCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty(); 

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Start date must be greater than current date.");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be greater than start date.");

        When(x => x.Percentage != null, () =>
        {
            RuleFor(x => x.Percentage)
                .InclusiveBetween(1, 100)
                .WithMessage("Percentage must be between 1 and 100.");
        });

        When(x => x.DiscountedPrice != null, () =>
        {
            RuleFor(x => x.DiscountedPrice)
                .GreaterThan(0)
                .WithMessage("Discounted price must be greater than 0.");
        });


        // must supply either percentage or discounted price
        RuleFor(x => new { x.Percentage, x.DiscountedPrice })
            .Must(x => x.Percentage != null || x.DiscountedPrice != null)
            .WithMessage("Must supply either percentage or discounted price.");

    }
}
