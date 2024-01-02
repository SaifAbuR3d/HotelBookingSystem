using FluentValidation;
using HotelBookingSystem.Application.DTOs.Common;

namespace HotelBookingSystem.Application.Validation.Common;

public class ResourceQueryParametersValidator : AbstractValidator<ResourceQueryParameters>
{
    public ResourceQueryParametersValidator()
    {
        RuleFor(x => x.PageNumber)
            .NotEmpty()
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .NotEmpty()
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be greater than or equal to 1.");

        RuleFor(x => x.SortOrder)
            .Must(x => string.IsNullOrEmpty(x) || x == "asc" || x == "desc")
            .WithMessage("Sort order must be empty or 'asc' or 'desc'.");

        RuleFor(x => x.SearchTerm)
            .Must(x => string.IsNullOrEmpty(x) || x.Length <= 100)
            .WithMessage("Search term must be empty or less than or equal to 100 characters.");
    }
}
