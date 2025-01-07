using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Contract.BusinessRules.Messaging;

namespace Contract.BusinessRules.Validation;

/// <summary>
/// [RegularExpression(@"0\d{9,10}")]
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class PhoneValidationAttribute : ValidationAttribute
{
    private const string PATTERN = @"0\d{9,10}";

    public override bool IsValid(object? value)
    {
        ErrorMessage = BusinessMessages.User.INVALID_PHONE;

        // RequiredAttribute should be used to assert a value is not empty.
        if (value is null)
            return true;

        string sValue = value.ToString()!;
        return Regex.IsMatch(sValue, PATTERN);
    }
}
