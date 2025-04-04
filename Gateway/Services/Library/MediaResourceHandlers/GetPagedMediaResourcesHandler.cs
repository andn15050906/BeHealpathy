using Contract.Domain.LibraryAggregate;
using Contract.Helpers;
using Contract.Requests.Library.MediaRequests;
using Contract.Requests.Library.MediaRequests.Dtos;
using Contract.Responses.Library;
using Contract.Responses.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gateway.Services.Library.MediaResourceHandlers;

public sealed class GetPagedMediaResourcesHandler : RequestHandler<GetPagedMediaResourcesQuery, PagedResult<MediaResourceModel>, HealpathyContext>
{
    public GetPagedMediaResourcesHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<PagedResult<MediaResourceModel>>> Handle(GetPagedMediaResourcesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                MediaResourceModel.MapExpression,
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
                entity.Media = medias.FirstOrDefault(_ => _.SourceId == entity.Id) ?? new MultimediaModel();

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<MediaResource, bool>>? GetPredicate(QueryMediaResourceDto dto)
    {
        if (dto.CreatorId is not null)
            return _ => _.CreatorId == dto.CreatorId && !_.IsDeleted;
        if (dto.Description is not null)
            return _ => _.Description.Contains(dto.Description) && !_.IsDeleted;
        if (dto.Artist is not null)
            return _ => _.Artist == dto.Artist && !_.IsDeleted;
        if (dto.Title is not null)
            return _ => _.Title.Contains(dto.Title) && !_.IsDeleted;
        if (dto.Type is not null)
            return _ => _.Type == dto.Type && !_.IsDeleted;
        return _ => !_.IsDeleted;
    }
}