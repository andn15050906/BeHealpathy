using System.Linq.Expressions;
using Contract.Domain.CourseAggregate;
using Contract.Responses.Shared;

namespace Contract.Responses.Courses;

public sealed class LectureModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Title { get; set; }                           = string.Empty;
    public string Content { get; set; }                         = string.Empty;
    public string ContentSummary { get; set; }                  = string.Empty;
    public bool IsPreviewable { get; set; }
    public int Index { get; set; }
    public string LectureType { get; set; }                     = string.Empty;
    public string MetaData { get; set; }                        = string.Empty;
    public Guid CourseId { get; set; }
    public IEnumerable<MultimediaModel> Materials { get; set; } = [];
    public IEnumerable<CommentModel> Comments { get; set; }     = [];






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
            ContentSummary = _.ContentSummary,
            IsPreviewable = _.IsPreviewable,
            CourseId = _.CourseId,
            Index = _.Index,
            LectureType = _.LectureType,
            MetaData = _.MetaData,
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
