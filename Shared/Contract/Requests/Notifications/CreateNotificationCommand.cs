using Contract.Requests.Notifications.Dtos;

namespace Contract.Requests.Notifications;

public sealed class CreateNotificationCommand : CreateCommand
{
    public CreateInstructorRequestDto? InstructorRequestRq { get; set; }
    public CreateWithdrawalRequestDto? WithdrawalRequestRq { get; set; }
    public Guid UserId { get; init; }



    public CreateNotificationCommand(Guid id, CreateInstructorRequestDto dto, Guid userId, bool isCompensating = false)
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
