using Contract.Requests.Notifications.Dtos;
using Contract.Requests.Shared.Base;

namespace Contract.Requests.Notifications;

public sealed class UpdateNotificationCommand : UpdateCommand
{
    public UpdateNotificationDto Rq { get; init; }
    public Guid UserId { get; init; }



    public UpdateNotificationCommand(UpdateNotificationDto rq, Guid userId, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
