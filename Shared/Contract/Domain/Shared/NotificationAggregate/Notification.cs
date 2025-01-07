using Contract.Domain.Shared.NotificationAggregate.Enums;
using Core.Domain;

namespace Contract.Domain.Shared.NotificationAggregate;

/// <summary>
/// Notification.Type is typed
/// </summary>
public sealed class Notification : CreationAuditedEntity
{
    // Attributes
    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public NotificationStatus Status { get; set; }

    // FKs
    public Guid? ReceiverId { get; set; }






#pragma warning disable CS8618
    public Notification()
    {

    }
#pragma warning restore CS8618

    public Notification(Guid id, Guid creatorId, string message, NotificationType type, Guid? receiverId)
    {
        Id = id;
        CreatorId = creatorId;
        Message = message is null ? string.Empty : message;
        Type = type;
        Status = NotificationStatus.Default;
        if (receiverId != null)
            ReceiverId = (Guid)receiverId;
    }
}