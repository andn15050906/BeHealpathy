using Contract.Helpers;
using Contract.Messaging.Models;
using Contract.Requests.Notifications;
using Gateway.Infrastructure;

namespace Gateway.Services.Notifications;

public sealed class CreateNotificationHandler : RequestHandler<CreateNotificationCommand, HealpathyContext>
{
    public CreateNotificationHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override Task<Result> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
