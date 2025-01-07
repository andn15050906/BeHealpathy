using Contract.Domain.UserAggregate;
using System.Linq.Expressions;

namespace Contract.Responses.Identity.UserModels;

public sealed class UserOverviewModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
    //...

    public static Expression<Func<User, UserOverviewModel>> MapExpression
        = _ => new UserOverviewModel
        {
            UserName = _.UserName,
            Email = _.Email,
        };
}
