using Contract.Domain.CommunityAggregate;
using Contract.Helpers;
using Contract.Requests.Community.ChatMessageRequests.Dtos;
using Contract.Requests.Community.ChatMessageRequests;
using Contract.Responses.Community;
using Contract.Responses.Shared;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Community.MessageHandlers;

public sealed class GetPagedChatMessagesHandler : RequestHandler<GetPagedChatMessagesQuery, PagedResult<ChatMessageModel>, HealpathyContext>
{
    public GetPagedChatMessagesHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<PagedResult<ChatMessageModel>>> Handle(GetPagedChatMessagesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                ChatMessageModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize,
                false,
                _ => _.Reactions
            );
            var result = await query.ExecuteWithOrderBy(_ => _.CreationTime);

            List<Guid> sourceIds = result.Items.Select(_ => _.Id).ToList();
            var medias = await _context.Multimedia
                .Where(_ => sourceIds.Contains(_.SourceId) && !_.IsDeleted)
                .Select(MultimediaModel.MapExpression)
                .ToListAsync();
            foreach (var entity in result.Items)
                entity.Attachments = medias.Where(_ => _.SourceId == entity.Id).ToList() ?? [];

            var reactions = await _context.MessageReactions
                .Where(_ => sourceIds.Contains(_.SourceId) || !_.IsDeleted)
                .Select(ReactionModel.MapExpression)
                .ToListAsync();
            foreach (var entity in result.Items)
                entity.Reactions = reactions.Where(_ => _.SourceId == entity.Id).ToList() ?? [];

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<ChatMessage, bool>>? GetPredicate(QueryChatMessageDto dto)
    {
        if (dto.CreatorId is not null)
            return _ => _.CreatorId == dto.CreatorId && !_.IsDeleted;
        if (dto.ConversationId is not null)
            return _ => _.ConversationId == dto.ConversationId && !_.IsDeleted;
        return _ => !_.IsDeleted;
    }
}