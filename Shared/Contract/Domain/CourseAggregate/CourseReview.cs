using Contract.Domain.Shared.MultimediaBase;
using Contract.Domain.Shared.ReviewBase;

namespace Contract.Domain.CourseAggregate;

public sealed class CourseReview : Review
{
    public CourseReview()
    {

    }

    public CourseReview(Guid id, Guid creatorId, Guid sourceId, string content, byte rating, List<Multimedia> medias)
        : base(id, creatorId, sourceId, content, rating, medias)
    {

    }
}