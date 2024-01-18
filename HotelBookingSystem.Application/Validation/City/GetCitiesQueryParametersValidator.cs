using FluentValidation;
using HotelBookingSystem.Application.DTOs.City.Query;
using HotelBookingSystem.Application.Validation.Common;

namespace HotelBookingSystem.Application.Validation.City;

public class GetCitiesQueryParametersValidator : AbstractValidator<GetCitiesQueryParameters>
{
    public GetCitiesQueryParametersValidator()
    {
        Include(new ResourceQueryParametersValidator());

        When(x => x.SortColumn != null, () =>
        {
            RuleFor(x => x.SortColumn)
             .Must(x => x.ToLower() == "id"
                     || x.ToLower() == "creationdate"
                     || x.ToLower() == "lastmodified"
                     || x.ToLower() == "name"
                     || x.ToLower() == "country"
                     || x.ToLower() == "postoffice"
                     || x.ToLower() == "hotels")

             .WithMessage("Sort column must be empty or 'id' or 'creationDate' or 'lastModified' or 'name' or 'country' or 'postOffice' or 'hotels'.");
        });
    }
}
