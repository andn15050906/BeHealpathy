using Contract.Requests.Progress.McqRequests.Dtos;
using Contract.Responses.Progress;

namespace Contract.Requests.Progress.McqRequests;

public sealed class GetPagedMcqQuestionsQuery : IRequest<Result<PagedResult<McqQuestionModel>>>
{
    public QueryMcqQuestionDto Rq { get; init; }
    public Guid UserId { get; init; }



    public GetPagedMcqQuestionsQuery(QueryMcqQuestionDto rq, Guid userId)
    {
        Rq = rq;
        UserId = userId;
    }
}
