using Contract.Domain.Shared.NotificationAggregate.Enums;

namespace Contract.Requests.Notifications.Dtos;

public sealed class CreateNotificationDto
{
    public string Message { get; init; } = string.Empty;
    public NotificationType Type { get; init; }
    public Guid ReceiverId { get; init; }
}
