using FluentValidation;
using HotelBookingSystem.Application.DTOs.Hotel.Query;
using HotelBookingSystem.Application.Validation.Common;

namespace HotelBookingSystem.Application.Validation.Hotel;

public class GetHotelsQueryParametersValidator : AbstractValidator<GetHotelsQueryParameters>
{
    public GetHotelsQueryParametersValidator()
    {
        Include(new ResourceQueryParametersValidator());
        When(x => x.SortColumn != null, () =>
        {
            RuleFor(x => x.SortColumn)
            .Must(x => x.ToLower() == "id"
                    || x.ToLower() == "creationdate"
                    || x.ToLower() == "lastmodified"
                    || x.ToLower() == "name"
                    || x.ToLower() == "owner"
                    || x.ToLower() == "starrate"
                    || x.ToLower() == "roomsnumber")
            .WithMessage("Sort column must be empty or 'id' or 'creationDate' or 'lastModified' or 'name' or 'owner' or 'starRate' or 'roomsNumber'.");
        });
    }
}
