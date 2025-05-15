using Contract.Domain.ToolAggregate;
using Contract.Helpers;
using Contract.Requests.Progress.DiaryNoteRequests;
using Contract.Requests.Progress.DiaryNoteRequests.Dtos;
using Contract.Responses.Progress;
using Contract.Responses.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gateway.Services.Progress.DiaryNoteHandlers;

public sealed class GetPagedDiaryNotesHandler : RequestHandler<GetPagedDiaryNotesQuery, PagedResult<DiaryNoteModel>, HealpathyContext>
{
    public GetPagedDiaryNotesHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<PagedResult<DiaryNoteModel>>> Handle(GetPagedDiaryNotesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                DiaryNoteModel.MapExpression,
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
                .ToListAsync(cancellationToken);
            foreach (var entity in result.Items)
                entity.Medias = medias.Where(_ => _.SourceId == entity.Id).ToList() ?? [];

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<DiaryNote, bool>>? GetPredicate(QueryDiaryNoteDto dto)
    {
        if (dto.CreatorId is not null)
            return _ => _.CreatorId == dto.CreatorId && !_.IsDeleted;
        if (dto.Title is not null)
            return _ => _.Title.Contains(dto.Title) && !_.IsDeleted;
        if (dto.Mood is not null)
            return _ => _.Mood == dto.Mood && !_.IsDeleted;

        if (dto.StartAfter is not null || dto.StartBefore is not null)
        {
            if (dto.StartBefore is null)
                return _ => _.CreationTime > dto.StartAfter && !_.IsDeleted;
            if (dto.StartAfter is null)
                return _ => _.CreationTime < dto.StartBefore && !_.IsDeleted;
            return _ => _.CreationTime > dto.StartAfter && _.CreationTime < dto.StartBefore && !_.IsDeleted;
        }
        if (dto.EndAfter is not null || dto.EndBefore is not null)
        {
            if (dto.EndAfter is null)
                return _ => _.CreationTime > dto.EndAfter && !_.IsDeleted;
            if (dto.EndBefore is null)
                return _ => _.CreationTime < dto.EndBefore && !_.IsDeleted;
            return _ => _.CreationTime > dto.EndAfter && _.CreationTime < dto.EndBefore && !_.IsDeleted;
        }

        return _ => !_.IsDeleted;
    }
}
