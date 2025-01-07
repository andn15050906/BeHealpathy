using Contract.Domain.Shared.CommentBase;

namespace Contract.Domain.CourseAggregate;

public sealed class LectureReaction : Reaction
{
    public LectureReaction() : base()
    {

    }

    public LectureReaction(Guid creatorId, Guid sourceEntityId, string content) : base(creatorId, sourceEntityId, content)
    {

    }
}