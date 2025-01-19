using Contract.Domain.Shared.ReactionBase;

namespace Contract.Domain.CourseAggregate;

public sealed class LectureReaction : Reaction
{
    public LectureReaction() : base()
    {

    }

    public LectureReaction(Guid id, Guid creatorId, Guid sourceEntityId, string content) : base(id, creatorId, sourceEntityId, content)
    {

    }
}