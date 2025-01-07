using Core.Domain.Interfaces;

namespace Core.Domain;

public abstract class TimeAuditedEntity : Entity, ICreationTimeAudited, IModificationTimeAudited
{
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
}