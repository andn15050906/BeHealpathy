using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Shared.BaseDtos.Comments;
using Contract.Requests.Shared.BaseRequests.Comments;

namespace Contract.Requests.Courses.LectureCommentRequests;

public sealed class CreateLectureCommentCommand : CreateCommentCommand
{
    public CreateLectureCommentCommand(Guid id, CreateCommentDto rq, Guid userId, List<Multimedia> medias, bool isCompensating = false)
        : base(id, rq, userId, medias, isCompensating)
    {
    }
}