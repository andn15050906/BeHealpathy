using System.ComponentModel.DataAnnotations;

namespace Contract.Requests.Identity.Dtos;

public sealed class CreateUserDto
{
    [EmailAddress(ErrorMessage = BusinessMessages.User.INVALID_EMAIL)]
    public string Email { get; set; } = null!;

    [StringLength(20, MinimumLength = 6, ErrorMessage = BusinessMessages.User.INVALID_USERNAME)]
    public string UserName { get; set; } = null!;

    [Required]
    [PasswordValidation]
    public string Password { get; set; } = null!;
}
