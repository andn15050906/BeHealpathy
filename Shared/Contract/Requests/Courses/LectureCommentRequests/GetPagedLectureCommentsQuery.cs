using Contract.Requests.Shared.BaseDtos.Comments;
using Contract.Requests.Shared.BaseRequests.Comments;

namespace Contract.Requests.Courses.LectureCommentRequests;

public sealed class GetPagedLectureCommentsQuery : GetPagedCommentsQuery
{
    public GetPagedLectureCommentsQuery(QueryCommentDto rq) : base(rq)
    {
    }
}