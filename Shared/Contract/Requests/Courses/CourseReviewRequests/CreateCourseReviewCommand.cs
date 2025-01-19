using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Shared.BaseDtos.Reviews;
using Contract.Requests.Shared.BaseRequests.Reviews;

namespace Contract.Requests.Library.CourseReviewRequests;

public sealed class CreateCourseReviewCommand : CreateReviewCommand
{
    public CreateCourseReviewCommand(Guid id, CreateReviewDto rq, Guid userId, List<Multimedia> medias, bool isCompensating = false)
        : base(id, rq, userId, medias, isCompensating)
    {
    }
}