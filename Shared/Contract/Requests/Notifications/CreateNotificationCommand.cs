using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Notifications.Dtos;
using Contract.Responses.Notifications;

namespace Contract.Requests.Notifications;

public sealed class CreateNotificationCommand : CreateCommand<NotificationModel>
{
    public Guid UserId { get; init; }

    public CreateAdvisorRequestDto? AdvisorRequestRq { get; set; }
    public Guid? AdvisorId { get; set; }
    public Multimedia? CV { get; set; }
    public List<Multimedia>? Certificates { get; set; }

    public CreateWithdrawalRequestDto? WithdrawalRequestRq { get; set; }

    public CreateAdminMessageDto? AdminMessageRq { get; set; }

    public CreateConversationInvitationDto? ConversationInvitationRq { get; set; }

    public CreateUserReportDto? UserReportRq { get; set; }

    public CreateUserBannedDto? UserBannedRq { get; set; }

    public CreateContentDisapprovedDto? ContentDisapprovedRq { get; set; }



    public CreateNotificationCommand(Guid id, Guid advisorId, CreateAdvisorRequestDto dto, Guid userId, Multimedia? cv, List<Multimedia>? certificates, bool isCompensating = false)
        : base(id, isCompensating)
    {
        UserId = userId;
        AdvisorRequestRq = dto;
        AdvisorId = advisorId;
        CV = cv;
        Certificates = certificates;
    }

    public CreateNotificationCommand(Guid id, CreateWithdrawalRequestDto dto, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        UserId = userId;
        WithdrawalRequestRq = dto;
    }

    public CreateNotificationCommand(Guid id, CreateAdminMessageDto dto, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        UserId = userId;
        AdminMessageRq = dto;
    }

    public CreateNotificationCommand(Guid id, CreateConversationInvitationDto dto, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        UserId = userId;
        ConversationInvitationRq = dto;
    }

    public CreateNotificationCommand(Guid id, CreateUserReportDto dto, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        UserId = userId;
        UserReportRq = dto;
    }

    public CreateNotificationCommand(Guid id, CreateUserBannedDto dto, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        UserId = userId;
        UserBannedRq = dto;
    }

    public CreateNotificationCommand(Guid id, CreateContentDisapprovedDto dto, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        UserId = userId;
        ContentDisapprovedRq = dto;
    }
}