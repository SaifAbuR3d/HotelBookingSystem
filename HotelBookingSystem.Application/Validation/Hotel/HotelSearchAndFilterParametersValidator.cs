using FluentValidation;
using HotelBookingSystem.Application.DTOs.Hotel.Query;
using HotelBookingSystem.Application.Validation.Common;

namespace HotelBookingSystem.Application.Validation.Hotel;

public class HotelSearchAndFilterParametersValidator : AbstractValidator<HotelSearchAndFilterParameters>
{
    public HotelSearchAndFilterParametersValidator()
    {
        Include(new ResourceQueryParametersValidator());

        When(x => x.SortColumn != null, () =>
        {
            RuleFor(x => x.SortColumn)
             .Must(x => x.ToLower() == "name"
                     || x.ToLower() == "rating"
                     || x.ToLower() == "price"
                     || x.ToLower() == "starrate")
             .WithMessage("Sort column must be empty or 'name' or 'rating' or 'price' or 'starRate'.");
        });

        RuleFor(x => x.CheckInDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Check-in date must be greater than or equal to today's date.");

        RuleFor(x => x.CheckOutDate)
            .GreaterThanOrEqualTo(x => x.CheckInDate)
            .WithMessage("Check-out date must be greater than or equal to check-in date.");

        RuleFor(x => x.Adults)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Adults must be greater than or equal to 1.");

        RuleFor(x => x.Children)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Children must be greater than or equal to 0.");

        RuleFor(x => x.Rooms)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Rooms must be greater than or equal to 1.");

        When(x => x.MinStarRating != null, () =>
        {
            RuleFor(x => x.MinStarRating)
                .InclusiveBetween(1, 5)
                .WithMessage("Min star rating must be between 1 and 5.");
        });

        When(x => x.MinPrice != null, () =>
        {
            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Min price must be greater than or equal to 0.");
        });

        When(x => x.MaxPrice != null, () =>
        {
            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(x => 0)
                .WithMessage("Max price must be greater than or equal to 0.")
                .GreaterThanOrEqualTo(x => x.MinPrice ?? 0)
                .WithMessage("Max price must be greater than or equal to min price.");
        });

        When(x => x.Amenities != null, () =>
        {
            RuleForEach(x => x.Amenities)
                .Length(1, 100)
                .WithMessage("each provided Amenity must be between 1 and 100 characters.");
        });

        When(x => x.RoomTypes != null, () =>
        {
            RuleForEach(x => x.RoomTypes)
                .IsInEnum()
                .WithMessage("provided RoomType is not valid"); 
        });

    }
}
