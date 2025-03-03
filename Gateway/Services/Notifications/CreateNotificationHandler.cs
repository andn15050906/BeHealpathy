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
        if (request.AdvisorRequestRq is not null)
        {
            try
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
                return Created();
            }
            catch (Exception ex)
            {
                return ServerError(ex.Message);
            }
        }
        else
        {
            return ServerError(string.Empty);
        }
    }

    private Notification AdaptAdvisorRequest(CreateNotificationCommand command)
    {
        var json = new
        {
            //...
            CV = command.CV is not null ? /*command.CV.Identifier*/ command.CV.Url : string.Empty,
            Introduction = command.AdvisorRequestRq!.Introduction ?? string.Empty,
            Experience = command.AdvisorRequestRq!.Experience ?? string.Empty,
            Certificates = command.Certificates is not null ? command.Certificates.Select(_ => _.Identifier) : []
        };

        return new Notification(command.Id, command.UserId, JsonSerializer.Serialize(json), NotificationType.RequestToBecomeAdvisor, PreSet.SystemUserId);
    }
}
