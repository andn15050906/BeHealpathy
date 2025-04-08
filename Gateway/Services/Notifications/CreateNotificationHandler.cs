using System.Text.Json;
using Contract.BusinessRules;
using Contract.Domain.Shared.MultimediaBase;
using Contract.Domain.Shared.NotificationAggregate;
using Contract.Domain.Shared.NotificationAggregate.Enums;
using Contract.Helpers;
using Contract.Requests.Notifications;
using Contract.Responses.Notifications;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Notifications;

public sealed class CreateNotificationHandler : RequestHandler<CreateNotificationCommand, NotificationModel, HealpathyContext>
{
    public CreateNotificationHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<NotificationModel>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.AdvisorRequestRq is not null)
            {
                var entity = AdaptAdvisorRequest(request);
                var notificationTask = _context.Notifications.InsertExt(entity);

                var medias = new List<Multimedia>();
                if (request.CV is not null)
                    medias.Add(request.CV);
                if (request.Certificates is not null)
                    medias.AddRange(request.Certificates);
                var mediaTask = _context.Multimedia.AddRangeAsync(medias);

                await Task.WhenAll(notificationTask, mediaTask);
            }
            else if (request.WithdrawalRequestRq is not null)
            {
                var entity = AdaptWithdrawalRequest(request);
                await _context.Notifications.InsertExt(entity);
            }
            else if (request.AdminMessageRq is not null)
            {
                var entity = AdaptAdminMessageRequest(request);
                await _context.Notifications.InsertExt(entity);
            }
            else if (request.ConversationInvitationRq is not null)
            {
                var entities = AdaptInviteMemberRequest(request);
                await _context.Notifications.AddRangeAsync(entities);
            }
            else if (request.UserReportRq is not null)
            {
                var entity = AdaptUserReportRequest(request);
                await _context.Notifications.InsertExt(entity);
            }
            else if (request.UserBannedRq is not null)
            {
                var entity = AdaptUserBannedRequest(request);
                await _context.Notifications.InsertExt(entity);
            }
            /*else if (request.ContentDisapprovedRq is not null)
            {

            }*/
            
            await _context.SaveChangesAsync();
            return Created();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private static Notification AdaptAdvisorRequest(CreateNotificationCommand command)
    {
        var dto = command.AdvisorRequestRq!;
        var message = new
        {
            CV = command.CV?.Url ?? string.Empty,
            Introduction = dto.Introduction ?? string.Empty,
            Experience = dto.Experience ?? string.Empty,
            Certificates = command.Certificates?.Select(c => c.Identifier) ?? []
        };

        return new Notification(
            command.Id,
            command.UserId,
            JsonSerializer.Serialize(message),
            NotificationType.RequestToBecomeAdvisor,
            PreSet.SystemUserId
        );
    }

    private static Notification AdaptWithdrawalRequest(CreateNotificationCommand command)
    {
        var dto = command.WithdrawalRequestRq!;
        return new Notification(
            command.Id,
            command.UserId,
            JsonSerializer.Serialize(dto),
            NotificationType.RequestWithdrawal,
            PreSet.SystemUserId
        );
    }

    private static Notification AdaptAdminMessageRequest(CreateNotificationCommand command)
    {
        var dto = command.AdminMessageRq!;
        return new Notification(
            command.Id,
            command.UserId,
            JsonSerializer.Serialize(dto.Message),
            NotificationType.AdminMessage,
            dto.ReceiverId
        );
    }

    private static IEnumerable<Notification> AdaptInviteMemberRequest(CreateNotificationCommand command)
    {
        var dto = command.ConversationInvitationRq!;
        return dto.UserIds.Select(_ => new Notification(
            Guid.NewGuid(),
            command.UserId,
            JsonSerializer.Serialize(dto),
            NotificationType.InviteMember,
            _
        ));
    }

    private static Notification AdaptUserReportRequest(CreateNotificationCommand command)
    {
        var dto = command.UserReportRq!;
        return new Notification(
            command.Id,
            command.UserId,
            JsonSerializer.Serialize(dto),
            NotificationType.ReportUser,
            dto.UserId
        );
    }

    private static Notification AdaptUserBannedRequest(CreateNotificationCommand command)
    {
        var dto = command.UserBannedRq!;
        return new Notification(
            command.Id,
            command.UserId,
            JsonSerializer.Serialize(dto),
            NotificationType.UserBanned,
            dto.UserId
        );
    }

    // Should be sent to content creator
    /*private static Notification AdaptContentDisapprovedRequest(CreateNotificationCommand command)
    {
        var dto = command.ContentDisapprovedRq!;
        return new Notification(
            command.Id,
            command.UserId,
            JsonSerializer.Serialize(dto),
            NotificationType.ContentDisapproved,
            PreSet.SystemUserId
        );
    }*/
}