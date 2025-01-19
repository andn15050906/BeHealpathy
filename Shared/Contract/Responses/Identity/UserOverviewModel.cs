using Contract.Domain.UserAggregate;
using Contract.Domain.UserAggregate.Enums;
using System.Linq.Expressions;

namespace Contract.Responses.Identity;

public sealed class UserOverviewModel
{
    public Guid Id { get; set; }

    public string Email { get; set; }
    public string FullName { get; set; }
    public string AvatarUrl { get; set; }
    public Role Role { get; set; }
    public string Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public static Expression<Func<User, UserOverviewModel>> MapExpression
        = _ => new UserOverviewModel
        {
            Id = _.Id,

            Email = _.Email,
            FullName = _.FullName,
            AvatarUrl = _.AvatarUrl,
            Role = _.Role,
            Bio = _.Bio,
            DateOfBirth = _.DateOfBirth
        };
}
