using Core.Domain.Interfaces;

namespace Core.Domain;

public abstract class CreationAuditedDomainObject : DomainObject, ICreationTimeAudited
{
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
}