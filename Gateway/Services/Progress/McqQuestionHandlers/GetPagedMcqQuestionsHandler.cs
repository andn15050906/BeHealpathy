using Contract.Domain.ProgressAggregate;
using Contract.Helpers;
using Contract.Requests.Progress.McqRequests;
using Contract.Requests.Progress.McqRequests.Dtos;
using Contract.Responses.Progress;
using System.Linq.Expressions;

namespace Gateway.Services.Library.McqQuestionHandlers;

public sealed class GetPagedMcqQuestionsHandler : RequestHandler<GetPagedMcqQuestionsQuery, PagedResult<McqQuestionModel>, HealpathyContext>
{
    public GetPagedMcqQuestionsHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<PagedResult<McqQuestionModel>>> Handle(GetPagedMcqQuestionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                McqQuestionModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize,
                false,
                _ => _.Answers
            );
            var result = await query.ExecuteWithOrderBy(_ => _.Id);

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<McqQuestion, bool>>? GetPredicate(QueryMcqQuestionDto dto)
    {
        return _ => _.SurveyId == dto.SurveyId && !_.IsDeleted;
    }
}
