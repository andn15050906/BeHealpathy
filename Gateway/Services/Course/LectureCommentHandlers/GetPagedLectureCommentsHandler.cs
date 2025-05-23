﻿using System.Linq.Expressions;
using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.LectureCommentRequests;
using Contract.Requests.Shared.BaseDtos.Comments;
using Contract.Responses.Shared;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Course.LectureCommentHandlers;
public sealed class GetPagedLectureCommentsHandler : RequestHandler<GetPagedLectureCommentsQuery, PagedResult<CommentModel>, HealpathyContext>
{
    public GetPagedLectureCommentsHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<PagedResult<CommentModel>>> Handle(GetPagedLectureCommentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                CommentModel.LectureCommentMapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize,
                false,
                _ => _.Creator
            );
            var result = await query.ExecuteWithOrderBy(_ => _.LastModificationTime);

            List<Guid> sourceIds = result.Items.Select(_ => _.Id).ToList();
            var medias = await _context.Multimedia
                .Where(_ => sourceIds.Contains(_.SourceId) && !_.IsDeleted)
                .Select(MultimediaModel.MapExpression)
                .ToListAsync();
            foreach (var entity in result.Items)
                entity.Medias = medias.Where(_ => _.SourceId == entity.Id).ToList() ?? [];

            var reactions = await _context.LectureReactions
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

    private static Expression<Func<LectureComment, bool>>? GetPredicate(QueryCommentDto dto)
    {
        if (dto.SourceId is not null)
            return _ => _.SourceId == dto.SourceId && !_.IsDeleted;
        if (dto.CreatorId is not null)
            return _ => _.CreatorId == dto.CreatorId && !_.IsDeleted;

        return _ => !_.IsDeleted;
    }
}