using Contract.Domain.UserAggregate.Enums;

namespace Contract.Responses.Identity.UserModels;

public sealed class UserModel
{
    public Guid Id { get; set; }

    public string Email { get; set; }
    public string FullName { get; set; }
    public string AvatarUrl { get; set; }
    public Role Role { get; set; }
    public bool IsApproved { get; set; }
    public string? LoginProvider { get; set; }
    public string Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int EnrollmentCount { get; set; }
}
