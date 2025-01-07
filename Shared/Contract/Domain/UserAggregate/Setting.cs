namespace Contract.Domain.UserAggregate;

/// <summary>
/// User_Setting
/// 
/// Language
/// Theme
/// Question Answer
/// Email Notification Enabled
/// </summary>
public sealed class Setting : CreationAuditedEntity
{
    // Attributes
    public string Title { get; set; }
    public string Choice { get; set; }

    // Navigations
    public User Creator { get; set; }
}
