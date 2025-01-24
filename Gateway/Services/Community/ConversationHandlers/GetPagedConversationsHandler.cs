using Contract.Domain.CommunityAggregate;
using Contract.Helpers;
using Contract.Requests.Community.ConversationRequests;
using Contract.Requests.Community.ConversationRequests.Dtos;
using Contract.Responses.Community;
using Contract.Responses.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gateway.Services.Community.ConversationHandlers;

public sealed class GetPagedConversationsHandler : RequestHandler<GetPagedConversationsQuery, PagedResult<ConversationModel>, HealpathyContext>
{
    public GetPagedConversationsHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<PagedResult<ConversationModel>>> Handle(GetPagedConversationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                ConversationModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize,
                false,
                _ => _.Members
            );
            var result = await query.ExecuteWithOrderBy(_ => _.Title);

            var sourceIds = result.Items.Select(_ => _.Id);
            var medias = await _context.Multimedia
                .Where(_ => sourceIds.Contains(_.SourceId) || !_.IsDeleted)
                .Select(MultimediaModel.MapExpression)
                .ToListAsync();

            foreach (var conversation in result.Items)
                conversation.AvatarUrl = medias.FirstOrDefault(_ => _.SourceId == conversation.Id)?.Url ?? string.Empty;

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<Conversation, bool>>? GetPredicate(QueryConversationDto dto)
    {
        // Exceptional
        if (dto.ConversationIds is not null)
            return _ => dto.ConversationIds.Contains(_.Id);



        if (dto.CreatorId is not null)
            return _ => _.CreatorId == dto.CreatorId;
        if (dto.Title is not null)
            return _ => _.Title.Contains(dto.Title) && !_.IsDeleted;
        if (dto.Members is not null)
            // Intersect
            return _ => _.Members.Select(_ => _.CreatorId).Intersect(dto.Members).Any();
        return _ => !_.IsDeleted;
    }
}