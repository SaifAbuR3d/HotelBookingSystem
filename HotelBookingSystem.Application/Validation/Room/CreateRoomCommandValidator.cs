using FluentValidation;
using HotelBookingSystem.Application.DTOs.Room;

namespace HotelBookingSystem.Application.Validation.Room;

using static Domain.Models.Constants.Common;
using static Domain.Models.Constants.Room;
public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(r => r.HotelName)
            .ValidName(MinNameLength, MaxNameLength);

        RuleFor(r => r.ChildrenCapacity)
            .InclusiveBetween(MinRoomCapacity, MaxRoomCapacity); 

        RuleFor(r => r.AdultsCapacity)
            .InclusiveBetween(MinRoomCapacity, MaxRoomCapacity);

        RuleFor(r => r.Price)
            .InclusiveBetween(MinRoomPrice, MaxRoomPrice);

        RuleFor(r => r.RoomNumber)
            .InclusiveBetween(MinRoomNumber, MaxRoomNumber);

        RuleFor(r => r.RoomType)
            .IsInEnum().WithMessage("provided RoomType is not valid");
    }
}
