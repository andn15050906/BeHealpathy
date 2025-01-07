using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Contract.BusinessRules.Messaging;

namespace Contract.BusinessRules.Validation;

/// <summary>
/// [StringLength(20, MinimumLength = 6)]
/// [RegularExpression(@"(?=.*[A-Z].*)(?=.*[a-z].*)(.*[0-9].*)"]
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class PasswordValidationAttribute : ValidationAttribute
{
    private const int MIN_LENGTH = 6;
    private const int MAX_LENGTH = 20;
    private const string PATTERN = @"(?=.*[A-Z].*)(?=.*[a-z].*)(.*[0-9].*)";

    public override bool IsValid(object? value)
    {
        // RequiredAttribute should be used to assert a value is not empty.
        if (value is null)
            return true;

        string sValue = value.ToString()!;
        if (sValue.Length < MIN_LENGTH || sValue.Length > MAX_LENGTH)
        {
            ErrorMessage = BusinessMessages.User.INVALID_PASSWORD_6TO20CHARS;
            return false;
        }

        ErrorMessage = BusinessMessages.User.INVALID_PASSWORD_REGEX;
        return Regex.IsMatch(sValue, PATTERN);
    }
}
