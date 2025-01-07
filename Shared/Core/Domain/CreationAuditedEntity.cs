using Core.Domain.Interfaces;

namespace Core.Domain;

public abstract class CreationAuditedEntity : Entity, ICreationTimeAudited
{
    public DateTime CreationTime { get; set; }
    public Guid CreatorId { get; protected set; }
}
