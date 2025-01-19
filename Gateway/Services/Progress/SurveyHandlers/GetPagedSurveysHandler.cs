using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.SurveyRequests;
using Contract.Requests.Progress.SurveyRequests.Dtos;
using Contract.Responses.Progress;
using System.Linq.Expressions;

namespace Gateway.Services.Library.SurveyHandlers;

public sealed class GetPagedSurveysHandler : RequestHandler<GetPagedSurveysQuery, PagedResult<SurveyModel>, HealpathyContext>
{
    public GetPagedSurveysHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<PagedResult<SurveyModel>>> Handle(GetPagedSurveysQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                SurveyModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize,
                false,
                _ => _.Questions
            );
            var result = await query.ExecuteWithOrderBy(_ => _.Name);

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<Survey, bool>>? GetPredicate(QuerySurveyDto dto)
    {
        if (dto.Name is not null)
            return _ => _.Name.Contains(dto.Name, StringComparison.OrdinalIgnoreCase) && !_.IsDeleted;

        return _ => !_.IsDeleted;
    }
}
