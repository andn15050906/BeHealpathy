using System.ComponentModel.DataAnnotations;
using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Identity.UserRequests.Dtos;

public sealed class UpdateUserDto
{
    public string? FullName { get; set; }

    public CreateMediaDto? Avatar { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [StringLength(1000, ErrorMessage = BusinessMessages.User.INVALID_BIO_LENGTH)]
    public string? Bio { get; set; }

    /*[PhoneValidation]
    public string? Phone { get; set; }*/



    [PasswordValidation]
    public string? CurrentPassword { get; set; }
    [PasswordValidation]
    public string? NewPassword { get; set; }
}
