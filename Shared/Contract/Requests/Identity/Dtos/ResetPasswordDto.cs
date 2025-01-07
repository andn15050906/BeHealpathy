using System.ComponentModel.DataAnnotations;

namespace Contract.Requests.Identity.Dtos;

public sealed class ResetPasswordDto
{
    public string Email { get; set; }

    public string Token { get; set; }

    [Required]
    [PasswordValidation]
    public string NewPassword { get; set; }
}