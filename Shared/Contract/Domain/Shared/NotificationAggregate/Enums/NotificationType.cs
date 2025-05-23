﻿namespace Contract.Domain.Shared.NotificationAggregate.Enums;

public enum NotificationType : byte
{
    Other,
    AdminMessage,
    RequestToBecomeAdvisor,
    AdvisorResponse,
    ReportGroup,
    GroupAdminReportedGroup,
    ReportCourse,
    AdvisorReportedCourse,
    RequestWithdrawal,
    UserBanned,
    InstructorRequestApproved,
    InviteMember,
    ReportUser,
    ContentDisapproved
}