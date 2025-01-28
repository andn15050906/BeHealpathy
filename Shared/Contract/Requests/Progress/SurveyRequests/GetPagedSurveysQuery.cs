using Contract.Requests.Progress.SurveyRequests.Dtos;
using Contract.Responses.Progress;

namespace Contract.Requests.Progress.SurveyRequests;

public sealed class GetPagedSurveysQuery : IRequest<Result<PagedResult<SurveyModel>>>
{
    public QuerySurveyDto Rq { get; init; }



    public GetPagedSurveysQuery(QuerySurveyDto rq)
    {
        Rq = rq;
    }
}
