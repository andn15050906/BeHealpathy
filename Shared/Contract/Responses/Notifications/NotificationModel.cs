using System.Linq.Expressions;
using Contract.Domain.Shared.NotificationAggregate;
using Contract.Domain.Shared.NotificationAggregate.Enums;

namespace Contract.Responses.Notifications;

public sealed class NotificationModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }



    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public NotificationStatus Status { get; set; }
    public Guid? ReceiverId { get; set; }



    public static Expression<Func<Notification, NotificationModel>> MapExpression
        = _ => new NotificationModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,

            Message = _.Message,
            Type = _.Type,
            Status = _.Status,
            ReceiverId = _.ReceiverId
        };
}
