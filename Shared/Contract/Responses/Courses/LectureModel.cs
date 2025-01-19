using System.Linq.Expressions;
using Contract.Domain.CourseAggregate;
using Contract.Domain.Shared.MultimediaBase;

namespace Contract.Responses.Courses;

public sealed class LectureModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsPreviewable { get; set; }
    public Guid SectionId { get; set; }
    public List<Multimedia> Materials { get; set; }






    public static Expression<Func<Lecture, LectureModel>> MapExpression
        = _ => new LectureModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            LastModifierId = _.LastModifierId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Title = _.Title,
            Content = _.Content,
            IsPreviewable = _.IsPreviewable,
            Materials = _.Materials
        };
}
