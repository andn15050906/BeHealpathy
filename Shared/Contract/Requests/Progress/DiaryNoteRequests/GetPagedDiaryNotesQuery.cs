using Contract.Requests.Progress.DiaryNoteRequests.Dtos;
using Contract.Responses.Progress;

namespace Contract.Requests.Progress.DiaryNoteRequests;

public sealed class GetPagedDiaryNotesQuery : IRequest<Result<PagedResult<DiaryNoteModel>>>
{
    public QueryDiaryNoteDto Rq { get; init; }
    public Guid UserId { get; init; }



    public GetPagedDiaryNotesQuery(QueryDiaryNoteDto rq, Guid userId)
    {
        Rq = rq;
        UserId = userId;
    }
}
