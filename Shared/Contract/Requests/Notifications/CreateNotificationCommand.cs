using Contract.Domain.Shared.MultimediaBase;
using Contract.Domain.Shared.NotificationAggregate.Enums;
using Contract.Requests.Notifications.Dtos;
using Contract.Responses.Notifications;

namespace Contract.Requests.Notifications;

public sealed class CreateNotificationCommand : CreateCommand<NotificationModel>
{
    public CreateAdvisorRequestDto? AdvisorRequestRq { get; set; }
    public Guid? AdvisorId { get; set; }
    public Multimedia? CV { get; set; }
    public List<Multimedia>? Certificates { get; set; }

    public CreateWithdrawalRequestDto? WithdrawalRequestRq { get; set; }
    public Guid UserId { get; init; }


    public string? Message { get; set; }
    public Guid? ReceiverId { get; set; }
    public NotificationType Type { get; set; }

    public CreateNotificationCommand(Guid id, Guid advisorId, CreateAdvisorRequestDto dto, Guid userId, Multimedia? cv, List<Multimedia>? certificates, bool isCompensating = false)
        : base(id, isCompensating)
    {
        AdvisorId = advisorId;
        AdvisorRequestRq = dto;
        UserId = userId;
        CV = cv;
        Certificates = certificates;
    }

    public CreateNotificationCommand(Guid id, CreateWithdrawalRequestDto dto, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        WithdrawalRequestRq = dto;
        UserId = userId;
    }

    public CreateNotificationCommand(Guid id, string message, Guid receiverId, NotificationType type, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Message = message;
        ReceiverId = receiverId;
        Type = type;
        UserId = userId;
    }
}
