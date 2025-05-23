﻿using Contract.Domain.Shared.NotificationAggregate.Enums;

namespace Contract.Requests.Notifications.Dtos;

public sealed class QueryNotificationDto : PagingQueryDto
{
    public Guid? CreatorId { get; init; }
    public Guid? ReceiverId { get; init; }
    public NotificationType? Type { get; init; }
    public NotificationStatus? Status { get; init; }
}
