using Contract.Messaging.Models;
using Contract.Requests.Notifications.Dtos;
using Contract.Responses.Notifications;

namespace Contract.Requests.Notifications;

public sealed class GetPagedNotificationsQuery : IRequest<Result<PagedResult<NotificationModel>>>
{
    public QueryNotificationDto Rq { get; init; }

    public Guid UserId { get; init; }



    public GetPagedNotificationsQuery(QueryNotificationDto rq, Guid userId)
    {
        Rq = rq;
        UserId = userId;
    }
}
