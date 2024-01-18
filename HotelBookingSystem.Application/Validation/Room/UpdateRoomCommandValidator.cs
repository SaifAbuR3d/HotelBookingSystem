using FluentValidation;
using HotelBookingSystem.Application.DTOs.Room.Command;

namespace HotelBookingSystem.Application.Validation.Room;

using static Domain.Models.Constants.Room;
public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator()
    {
        RuleFor(r => r.ChildrenCapacity)
            .InclusiveBetween(MinRoomCapacity, MaxRoomCapacity);

        RuleFor(r => r.AdultsCapacity)
            .InclusiveBetween(MinRoomCapacity, MaxRoomCapacity);

        RuleFor(r => r.RoomNumber)
            .InclusiveBetween(MinRoomNumber, MaxRoomNumber);
    }
}
