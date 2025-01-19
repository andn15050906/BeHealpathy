using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Shared.BaseDtos.Comments;
using Contract.Requests.Shared.BaseRequests.Comments;

namespace Contract.Requests.Library.LectureCommentRequests;

public sealed class UpdateLectureCommentCommand : UpdateCommentCommand
{
    public UpdateLectureCommentCommand(
        UpdateCommentDto rq, Guid userId,
        List<Multimedia> addedMedias, List<Guid> removedMedias, bool isCompensating = false)
        : base(rq, userId, addedMedias, removedMedias, isCompensating)
    {
    }
}