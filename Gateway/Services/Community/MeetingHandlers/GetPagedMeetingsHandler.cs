using Contract.Domain.CommunityAggregate;
using Contract.Helpers;
using Contract.Requests.Community.MeetingRequests;
using Contract.Requests.Community.MeetingRequests.Dtos;
using Contract.Responses.Community;
using System.Linq.Expressions;

namespace Gateway.Services.Community.MeetingHandlers;

public sealed class GetPagedMeetingsHandler : RequestHandler<GetPagedMeetingsQuery, PagedResult<MeetingModel>, HealpathyContext>
{
    public GetPagedMeetingsHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<PagedResult<MeetingModel>>> Handle(GetPagedMeetingsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                MeetingModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize,
                false,
                _ => _.Participants
            );
            var result = await query.ExecuteWithOrderBy(_ => _.StartAt);

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<Meeting, bool>>? GetPredicate(QueryMeetingDto dto)
    {
        if (dto.CreatorId is not null)
            return _ => _.CreatorId == dto.CreatorId && !_.IsDeleted;
        if (dto.Title is not null)
            return _ => _.Title.Contains(dto.Title) && !_.IsDeleted;

        if (dto.StartAfter is not null || dto.StartBefore is not null)
        {
            if (dto.StartBefore is null)
                return _ => _.StartAt > dto.StartAfter && !_.IsDeleted;
            if (dto.StartAfter is null)
                return _ => _.StartAt < dto.StartBefore && !_.IsDeleted;
            return _ => _.StartAt > dto.StartAfter && _.StartAt < dto.StartBefore && !_.IsDeleted;
        }
        if (dto.EndAfter is not null || dto.EndBefore is not null)
        {
            if (dto.EndAfter is null)
                return _ => _.EndAt > dto.EndAfter && !_.IsDeleted;
            if (dto.EndBefore is null)
                return _ => _.EndAt < dto.EndBefore && !_.IsDeleted;
            return _ => _.EndAt > dto.EndAfter && _.EndAt < dto.EndBefore && !_.IsDeleted;
        }

        if (dto.MaxParticipants is not null)
            return _ => _.MaxParticipants < dto.MaxParticipants && !_.IsDeleted;

        if (dto.Participants is not null)
            // Intersect
            return _ => _.Participants.Select(_ => _.CreatorId).Intersect(dto.Participants).Any() && !_.IsDeleted;

        return _ => !_.IsDeleted;
    }
}