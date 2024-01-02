using FluentValidation;
using HotelBookingSystem.Application.DTOs.Review.Query;
using HotelBookingSystem.Application.Validation.Common;
using System.Security.Cryptography.X509Certificates;

namespace HotelBookingSystem.Application.Validation.Review;

public class GetHotelReviewsQueryParametersValidator : AbstractValidator<GetHotelReviewsQueryParameters>
{
    public GetHotelReviewsQueryParametersValidator()
    {
        Include(new ResourceQueryParametersValidator());
        When(x => x.SortColumn != null, () =>
        {
            RuleFor(x => x.SortColumn)
                .Must(x => x.ToLower() == "id"
                   || x.ToLower() == "creationdate"
                   || x.ToLower() == "lastmodified"
                   || x.ToLower() == "rating")

                .WithMessage("Sort column must be either 'id' or 'creationDate' or 'lastModified' or 'rating'");
        }); 
    }
}
