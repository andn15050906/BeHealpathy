using Contract.Domain.Shared.NotificationAggregate.Enums;

namespace Contract.Requests.Notifications.Dtos;

public sealed class UpdateNotificationDto
{
    public Guid Id { get; init; }
    public NotificationStatus Status { get; init; }
}
