namespace Contract.Domain.UserAggregate;

/// <summary>
/// SourceId is not FK
/// </summary>
public sealed class Preference : CreationAuditedEntity
{
    // Attributes
    public Guid SourceId { get; set; }
    public string Value { get; set; }

    // Navigations
    public User Creator { get; set; }
}
