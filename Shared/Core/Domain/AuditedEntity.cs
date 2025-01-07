namespace Core.Domain;

public abstract class AuditedEntity : TimeAuditedEntity
{
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
}