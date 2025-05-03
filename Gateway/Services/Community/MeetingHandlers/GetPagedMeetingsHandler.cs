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

        if (dto.Start is not null && dto.End is not null)
        { 
            if (dto.Participants is not null)
            {
                return _ => _.StartAt >= dto.Start && _.EndAt <= dto.End && !_.IsDeleted && _.Participants.Select(_ => _.CreatorId).Intersect(dto.Participants).Any();
            }    
            return _ => _.StartAt >= dto.Start && _.EndAt <= dto.End && !_.IsDeleted;
        }

        if (dto.MaxParticipants is not null)
            return _ => _.MaxParticipants < dto.MaxParticipants && !_.IsDeleted;

        if (dto.Participants is not null)
            // Intersect
            return _ => _.Participants.Select(_ => _.CreatorId).Intersect(dto.Participants).Any() && !_.IsDeleted;

        return _ => !_.IsDeleted;
    }
}