using Contract.Domain.Shared.CommentBase;
using Contract.Domain.Shared.MultimediaBase;

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

    public LectureComment(Guid id, Guid creatorId, Guid sourceId, string content, List<Multimedia> medias)
        : base(id, creatorId, sourceId, content, medias)
    {

    }
#pragma warning restore CS8618
}