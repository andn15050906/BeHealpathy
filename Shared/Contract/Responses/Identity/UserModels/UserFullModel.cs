using Contract.Domain.UserAggregate;
using Contract.Domain.UserAggregate.Enums;
using System.Linq.Expressions;

namespace Contract.Responses.Identity.UserModels;

public sealed class UserFullModel
{
    public Guid Id { get; set; }

    public string UserName { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string AvatarUrl { get; set; }
    public Role Role { get; set; }
    public bool IsVerified { get; set; }
    public bool IsApproved { get; set; }
    public bool IsBanned { get; set; }
    public string? LoginProvider { get; set; }
    public string Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int EnrollmentCount { get; set; }

    public DateTime CreationTime { get; set; }






    public static UserFullModel From(User user)
    {
        return new UserFullModel
        {
            Id = user.Id,

            UserName = user.UserName,
            Email = user.Email,
            FullName = user.FullName,
            AvatarUrl = user.AvatarUrl,
            Role = user.Role,
            IsVerified = user.IsVerified,
            IsApproved = user.IsApproved,
            Bio = user.Bio,
            DateOfBirth = (DateTime)user.DateOfBirth!,
            EnrollmentCount = user.EnrollmentCount,

            CreationTime = user.CreationTime
        };
    }

    public static Expression<Func<User, UserFullModel>> MapExpression
        = _ => new UserFullModel
        {
            Id = _.Id,
            CreationTime = _.CreationTime,

            UserName = _.UserName,
            Email = _.Email,
            FullName = _.FullName,
            AvatarUrl = _.AvatarUrl,
            Role = _.Role,
            IsVerified = _.IsVerified,
            IsApproved = _.IsApproved,
            IsBanned = _.IsBanned,
            Bio = _.Bio,
            DateOfBirth = _.DateOfBirth,
            EnrollmentCount = _.EnrollmentCount
        };
}
