using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Shared.BaseDtos.Reviews;
using Contract.Requests.Shared.BaseRequests.Reviews;

namespace Contract.Requests.Library.CourseReviewRequests;

public sealed class UpdateCourseReviewCommand : UpdateReviewCommand
{
    public UpdateCourseReviewCommand(UpdateReviewDto rq, Guid userId, List<Multimedia> addedMedias, List<Guid> removedMedias, bool isCompensating = false)
        : base(rq, userId, addedMedias, removedMedias, isCompensating)
    {
    }
}
