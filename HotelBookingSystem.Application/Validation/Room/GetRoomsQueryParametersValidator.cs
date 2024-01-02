using FluentValidation;
using HotelBookingSystem.Application.DTOs.Room.Query;
using HotelBookingSystem.Application.Validation.Common;

namespace HotelBookingSystem.Application.Validation.Room;

public class GetRoomsQueryParametersValidator : AbstractValidator<GetRoomsQueryParameters>
{
    public GetRoomsQueryParametersValidator()
    {
        Include(new ResourceQueryParametersValidator());

        When(x => x.SortColumn != null, () =>
        {
            RuleFor(x => x.SortColumn)
             .Must(x => x.ToLower() == "id"
                     || x.ToLower() == "creationdate"
                     || x.ToLower() == "lastmodified"
                     || x.ToLower() == "roomnumber"
                     || x.ToLower() == "adultscapacity"
                     || x.ToLower() == "childrencapacity"
                     || x.ToLower() == "hotelname")

             .WithMessage("Sort column must be empty or 'id' or 'creationDate' or 'lastModified' or 'roomNumber' or 'adultsCapacity' or 'childrenCapacity' or 'hotelName'.");
        });
    }
}
