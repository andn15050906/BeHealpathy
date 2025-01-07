namespace Contract.Domain.Shared.NotificationAggregate.Enums;

public enum NotificationType : byte
{
    Other,

    AdminMessage,

    RequestToBecomeInstructor,
    InstructorResponse,

    ReportGroup,
    GroupAdminReportedGroup,

    ReportCourse,
    InstructorReportedCourse,

    InviteMember,

    RequestWithdrawal
}