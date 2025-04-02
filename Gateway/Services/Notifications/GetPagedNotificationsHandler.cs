using System.Linq.Expressions;
using Contract.Domain.Shared.NotificationAggregate;
using Contract.Helpers;
using Contract.Requests.Notifications;
using Contract.Requests.Notifications.Dtos;
using Contract.Responses.Notifications;

namespace Gateway.Services.Notifications;

public sealed class GetPagedNotificationsHandler : RequestHandler<GetPagedNotificationsQuery, PagedResult<NotificationModel>, HealpathyContext>
{
    public GetPagedNotificationsHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<PagedResult<NotificationModel>>> Handle(GetPagedNotificationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                NotificationModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize
            );

            var result = await query.ExecuteWithOrderBy(_ => _.CreationTime);
            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<Notification, bool>>? GetPredicate(QueryNotificationDto dto)
    {
        if (dto.CreatorId is not null)
            return _ => _.CreatorId == dto.CreatorId && !_.IsDeleted;
        if (dto.ReceiverId is not null)
            return _ => _.ReceiverId == dto.ReceiverId && !_.IsDeleted;
        if (dto.Type is not null)
            return _ => _.Type == dto.Type && !_.IsDeleted;
        if (dto.Status is not null)
            return _ => _.Status == dto.Status && !_.IsDeleted;
        return _ => !_.IsDeleted;
    }
}