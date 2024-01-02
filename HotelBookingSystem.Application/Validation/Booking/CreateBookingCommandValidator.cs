using FluentValidation;
using HotelBookingSystem.Application.DTOs.Booking;

namespace HotelBookingSystem.Application.Validation.Booking;

using static Domain.Models.Constants.Room;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(b => b.GuestId)
            .NotEmpty(); 
        
        RuleFor(b => b.RoomId)
            .NotEmpty();

        RuleFor(b => b.NumberOfAdults)
            .InclusiveBetween(0, MaxRoomCapacity); 

        RuleFor(b => b.NumberOfChildren)
            .InclusiveBetween(0, MaxRoomCapacity);

        RuleFor(b => b.CheckInDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Check-in date must be in the future");

        RuleFor(b => b.CheckOutDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(b => b.CheckInDate)
            .WithMessage("Check-out date must be after check-in date");
    }
}
