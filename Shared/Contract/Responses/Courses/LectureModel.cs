using System.Linq.Expressions;
using Contract.Domain.CourseAggregate;
using Contract.Domain.Shared.MultimediaBase;
using Contract.Responses.Shared;

namespace Contract.Responses.Courses;

public sealed class LectureModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ContentSummary { get; set; } = string.Empty;
    public bool IsPreviewable { get; set; }
    public Guid CourseId { get; set; }
    public IEnumerable<MultimediaModel> Materials { get; set; } = [];
    public IEnumerable<CommentModel> Comments { get; set; } = [];






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
            CourseId = _.CourseId,
            //Materials = _.Materials,
            Comments = _.Comments.Select(_ => new CommentModel
            {
                Id = _.Id,
                CreatorId = _.CreatorId,
                LastModifierId = _.LastModifierId,
                CreationTime = _.CreationTime,
                LastModificationTime = _.LastModificationTime,
                SourceId = _.SourceId,
                Content = _.Content,
                Status = _.Status,
                //Medias
                //Reactions
            }).ToList()
        };
}
