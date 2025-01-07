using System.ComponentModel.DataAnnotations;

namespace Contract.Requests.Identity.Dtos;

public sealed class SignInDto
{
    public string? UserName { get; set; }

    [EmailAddress(ErrorMessage = BusinessMessages.User.INVALID_EMAIL)]
    public string? Email { get; set; }

    [Required]
    [PasswordValidation]
    public string Password { get; set; } = null!;
}
