using FluentValidation;
using HotelBookingSystem.Application.DTOs.Review;

namespace HotelBookingSystem.Application.Validation.Review;

using static Domain.Models.Constants.Review;
public class CreateOrUpdateReviewCommandValidator : AbstractValidator<CreateOrUpdateReviewCommand>
{
    public CreateOrUpdateReviewCommandValidator()
    {
        RuleFor(x => x.Rating)
            .NotEmpty()
            .InclusiveBetween(MinRating, MaxRating)
            .WithMessage($"Rating must be between {MinRating} and {MaxRating}");

        When(x => x.Title != null, () =>
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(MinReviewTitleLength, MaxReviewTitleLength)
                .WithMessage($"Title must be between {MinReviewTitleLength} and {MaxReviewTitleLength} characters long");
        });

        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(MinReviewDescriptionLength, MaxReviewDescriptionLength)
            .WithMessage($"Description must be between {MinReviewDescriptionLength} and {MaxReviewDescriptionLength} characters long");
    }
}
