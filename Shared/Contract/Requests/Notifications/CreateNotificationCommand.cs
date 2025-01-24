using Contract.Requests.Notifications.Dtos;
using Contract.Responses.Notifications;

namespace Contract.Requests.Notifications;

public sealed class CreateNotificationCommand : CreateCommand<NotificationModel>
{
    public CreateAdvisorRequestDto? InstructorRequestRq { get; set; }
    public CreateWithdrawalRequestDto? WithdrawalRequestRq { get; set; }
    public Guid UserId { get; init; }



    public CreateNotificationCommand(Guid id, CreateAdvisorRequestDto dto, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        InstructorRequestRq = dto;
        UserId = userId;
    }

    public CreateNotificationCommand(Guid id, CreateWithdrawalRequestDto dto, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        WithdrawalRequestRq = dto;
        UserId = userId;
    }
}
