using Contract.Domain.UserAggregate;
using System.Linq.Expressions;

namespace Contract.Responses.Identity;

public sealed class UserMinModel
{
    public Guid Id { get; set; }

    public string FullName { get; set; }
    public string AvatarUrl { get; set; }

    public static Expression<Func<User, UserMinModel>> MapExpression
        = _ => new UserMinModel
        {
            Id = _.Id,

            FullName = _.FullName,
            AvatarUrl = _.AvatarUrl
        };
}
