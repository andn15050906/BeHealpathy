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



#pragma warning disable CS8618
    public Preference(Guid id, Guid creatorId, Guid sourceId, string value)
    {
        Id = id;
        CreatorId = creatorId;
        SourceId = sourceId;
        Value = value;
    }
#pragma warning restore CS8618
}
