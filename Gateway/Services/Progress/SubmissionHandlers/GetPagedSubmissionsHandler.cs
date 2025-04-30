using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.SubmissionRequests;
using Contract.Requests.Progress.SubmissionRequests.Dtos;
using Contract.Responses.Progress;
using System.Linq.Expressions;

namespace Gateway.Services.Progress.SubmissionHandlers;

public sealed class GetPagedSubmissionsHandler : RequestHandler<GetPagedSubmissionsQuery, PagedResult<SubmissionModel>, HealpathyContext>
{
    public GetPagedSubmissionsHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<PagedResult<SubmissionModel>>> Handle(GetPagedSubmissionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                SubmissionModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize,
                false,
                _ => _.Choices
            );
            var result = await query.ExecuteWithOrderBy(_ => _.LastModificationTime);

            List<Guid> sourceIds = result.Items.Select(_ => _.Id).ToList();
            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<Submission, bool>>? GetPredicate(QuerySubmissionDto dto)
    {
        if (dto.Id is not null)
            return _ => _.Id == dto.Id && !_.IsDeleted;
        if (dto.CreatorId is not null)
            return _ => _.CreatorId == dto.CreatorId && !_.IsDeleted;
        if (dto.SurveyId is not null)
            return _ => _.SurveyId == dto.SurveyId && !_.IsDeleted;

        return _ => !_.IsDeleted;
    }
}
