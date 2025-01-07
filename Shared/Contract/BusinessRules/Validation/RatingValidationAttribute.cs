using System.ComponentModel.DataAnnotations;
using Contract.BusinessRules.Messaging;

namespace Contract.BusinessRules.Validation;

/// <summary>
/// [Range(1, 5)]
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class RatingValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        ErrorMessage = BusinessMessages.Course.INVALID_RATING;

        // RequiredAttribute should be used to assert a value is not empty.
        if (value is null)
            return true;

        if (!byte.TryParse(value.ToString(), out var result))
            return false;
        return result > 0 && result < 6;
    }
}
