using System.Text.Json;
using Contract.BusinessRules;
using Contract.Domain.Shared.MultimediaBase;
using Contract.Domain.Shared.NotificationAggregate;
using Contract.Domain.Shared.NotificationAggregate.Enums;
using Contract.Helpers;
using Contract.Requests.Notifications;
using Contract.Responses.Notifications;
using Infrastructure.DataAccess.SQLServer.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Notifications;

public sealed class CreateNotificationHandler : RequestHandler<CreateNotificationCommand, NotificationModel, HealpathyContext>
{
    public CreateNotificationHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<NotificationModel>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Notification notification;

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
                await _context.SaveChangesAsync(cancellationToken);

                notification = entity;
            }
            else
            {
                var receiverExists = await _context.Users.AnyAsync(u => u.Id == request.ReceiverId, cancellationToken);
                if (!receiverExists)
                {
                    return BadRequest("ReceiverId not found.");
                }

                notification = Adapt(request);
                await _context.Notifications.InsertExt(notification);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var notificationModel = new NotificationModel
            {
                Id = notification.Id,
                Message = notification.Message,
                Type = notification.Type,
                ReceiverId = notification.ReceiverId,
                CreationTime = notification.CreationTime
            };

            return Created(notificationModel);
        }
        catch (Exception ex)
        {
            return ServerError(string.Empty);
        }
    }

    private Notification AdaptAdvisorRequest(CreateNotificationCommand command)
    {
        var json = new
        {
            CV = command.CV?.Url ?? string.Empty,
            Introduction = command.AdvisorRequestRq!.Introduction ?? string.Empty,
            Experience = command.AdvisorRequestRq!.Experience ?? string.Empty,
            Certificates = command.Certificates?.Select(c => c.Identifier) ?? Enumerable.Empty<string>()
        };

        return new Notification(
            command.Id,
            command.UserId,
            JsonSerializer.Serialize(json),
            NotificationType.RequestToBecomeAdvisor,
            PreSet.SystemUserId
        );
    }

    private Notification Adapt(CreateNotificationCommand command)
    {
        return new Notification(
            command.Id,
            command.UserId,
            command.Message ?? string.Empty,
            command.Type,
            command.ReceiverId ?? Guid.Empty
        );
    }
}
