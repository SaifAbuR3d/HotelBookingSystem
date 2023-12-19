using FluentValidation;

namespace HotelBookingSystem.Application.Validation;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ValidName<T>(this IRuleBuilder<T, string> ruleBuilder,
        int minLength,
        int maxLength)
    {
        return ruleBuilder
         .NotEmpty().WithMessage("'{PropertyName}' is required.")
         .Matches(@"^[A-Za-z(),\-\s]*$").WithMessage("'{PropertyName}' should only contain letters.")
         .Length(minLength, maxLength);
    }

    public static IRuleBuilderOptions<T, string> ValidNumericString<T>(this IRuleBuilder<T, string> ruleBuilder,
               short length)
    {
        return ruleBuilder
         .NotEmpty().WithMessage("'{PropertyName}' is required.")
         .Matches($"^[0-9]{{{length}}}$").WithMessage("'{PropertyName}' " + $"must be exactly {length}-digits."); 
    }

}
