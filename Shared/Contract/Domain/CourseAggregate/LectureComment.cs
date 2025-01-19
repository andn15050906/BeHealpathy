using Contract.Domain.Shared.CommentBase;

namespace Contract.Domain.CourseAggregate;

public sealed class LectureComment : Comment
{
    // Navigations
    public List<LectureComment> Replies { get; set; }
    public List<LectureReaction> Reactions { get; set; }






#pragma warning disable CS8618
    public LectureComment() : base()
    {

    }

    public LectureComment(Guid id, Guid creatorId, Guid sourceId, string content)
        : base(id, creatorId, sourceId, content)
    {

    }
#pragma warning restore CS8618
}