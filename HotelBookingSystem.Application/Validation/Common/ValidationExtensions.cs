using FluentValidation;

namespace HotelBookingSystem.Application.Validation.Common;

/// <summary>
/// Set of Extension methods for <see cref="FluentValidation.IRuleBuilder{T, TProperty}"/>
/// </summary>
public static class ValidationExtensions
{

    /// <summary>
    /// Validates that a string property contains only letters and has a length within the specified range.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validation rule is being defined.</param>
    /// <param name="minLength">The minimum length of the string. Defaults to 2 characters.</param>
    /// <param name="maxLength">The maximum length of the string. Defaults to 35 characters.</param>
    /// <remarks>
    /// This validation method checks that the property is not empty,
    /// contains only letters (including special characters like commas, hyphens, and spaces),
    /// and has a length within the specified range.
    /// <para>
    /// If the validation fails, appropriate error messages are returned to indicate the specific violation 
    /// </para>
    /// </remarks>
    /// <returns>Rule builder options for further chaining validation rules.</returns>
    public static IRuleBuilderOptions<T, string> ValidName<T>(this IRuleBuilder<T, string> ruleBuilder,
        int minLength = 2,
        int maxLength = 35)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("'{PropertyName}' is required.")
            .Matches(@"^[A-Za-z(),\-\s]*$").WithMessage("'{PropertyName}' should only contain letters.")
            .Length(minLength, maxLength);
    }

    /// <summary>
    /// Extension method to validate if a string is a numeric string of a specific length.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validation rule is being defined.</param>
    /// <param name="length">The exact length the numeric string should be.</param>
    /// <returns>Rule builder options for further chaining validation rules.</returns>
    public static IRuleBuilderOptions<T, string> ValidNumericString<T>(this IRuleBuilder<T, string> ruleBuilder,
               int length)
    {
        return ruleBuilder
         .NotEmpty().WithMessage("'{PropertyName}' is required.")
         .Matches($"^[0-9]{{{length}}}$").WithMessage("'{PropertyName}' " + $"must be exactly {length}-digits.");
    }

    /// <summary>
    /// Extension method to validate a password for complexity requirements.
    /// It ensures that the password is a minimum of 6 characters in length and includes
    /// at least one uppercase letter, one lowercase letter, one number, and one special character.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validation rule is being defined.</param>
    /// <returns>Rule builder options for further chaining validation rules.</returns>
    public static IRuleBuilderOptions<T, string> StrongPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder 
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password Must be at least 8 characters")
            .Matches("[A-Z]").WithMessage("Password must include UPPERCASE letters")
            .Matches("[a-z]").WithMessage("Password must include lowercase letters")
            .Matches("[0-9]").WithMessage("Password must include digits")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must include special characters");
    }

}
