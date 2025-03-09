using Contract.Domain.UserAggregate;
using Contract.Domain.UserAggregate.Enums;
using System.Linq.Expressions;

namespace Contract.Responses.Identity;

public sealed class UserFullModel
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public Role Role { get; set; }
    public bool IsVerified { get; set; }
    public bool IsApproved { get; set; }
    public IEnumerable<string> LoginProviders { get; set; } = [];
    public string Bio { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }

    public Guid? RoadmapId { get; set; }
    public int EnrollmentCount { get; set; }

    public DateTime CreationTime { get; set; }
    public IEnumerable<PreferenceModel> Preferences { get; set; } = [];
    public IEnumerable<SettingModel> Settings { get; set; } = [];






    public static UserFullModel From(User user)
    {
        return new UserFullModel
        {
            Id = user.Id,
            CreationTime = user.CreationTime,

            UserName = user.UserName,
            Email = user.Email,
            FullName = user.FullName,
            AvatarUrl = user.AvatarUrl,
            Role = user.Role,
            IsVerified = user.IsVerified,
            IsApproved = user.IsApproved,
            Bio = user.Bio,
            DateOfBirth = user.DateOfBirth,

            RoadmapId = user.RoadmapId,
            EnrollmentCount = user.EnrollmentCount,
            LoginProviders = user.UserLogins?.Select(_ => _.LoginProvider) ?? [],
            Settings = user.Settings?.Select(_ => new SettingModel
            {
                Title = _.Title,
                Choice = _.Choice
            }) ?? [],
            Preferences = user.Preferences?.Select(_ => new PreferenceModel
            {
                SourceId = _.SourceId,
                Value = _.Value
            }) ?? []
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
            Bio = _.Bio,
            DateOfBirth = _.DateOfBirth,

            RoadmapId = _.RoadmapId,
            EnrollmentCount = _.EnrollmentCount,
            LoginProviders = _.UserLogins.Select(x => x.LoginProvider),
            Settings = _.Settings.Select(_ => new SettingModel
            {
                Title = _.Title,
                Choice = _.Choice
            }),
            Preferences = _.Preferences.Select(_ => new PreferenceModel
            {
                SourceId = _.SourceId,
                Value = _.Value
            })
        };
}
