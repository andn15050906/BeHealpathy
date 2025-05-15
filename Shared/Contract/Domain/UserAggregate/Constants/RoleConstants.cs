using Contract.Domain.UserAggregate.Enums;

namespace Contract.Domain.UserAggregate.Constants;

public sealed class RoleConstants
{
    public const string MEMBER = nameof(Role.Member);
    public const string ADVISOR = nameof(Role.Advisor);
    public const string ADMIN = nameof(Role.Admin);
}