using Contract.Domain.Shared.CommentBase;
using Contract.Domain.Shared.CommentBase.Enums;
using Contract.Domain.Shared.MultimediaBase;
using System.Linq.Expressions;

namespace Contract.Responses.Shared.Comments;

public class CommentModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Content { get; set; }
    public CommentStatus Status { get; set; }
    public List<Multimedia> Medias { get; set; }

    public Guid SourceId { get; set; }






    public static Expression<Func<Comment, CommentModel>> MapExpression
        = _ => new CommentModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            LastModifierId = _.LastModifierId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Content = _.Content,
            Status = _.Status,
            Medias = _.Medias,

            SourceId = _.SourceId
        };
}
