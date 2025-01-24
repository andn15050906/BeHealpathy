using Contract.Domain.Shared.CommentBase;
using Contract.Domain.Shared.CommentBase.Enums;
using System.Linq.Expressions;

namespace Contract.Responses.Shared;

public class CommentModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public Guid SourceId { get; set; }

    public string Content { get; set; }
    public CommentStatus Status { get; set; }
    public IEnumerable<MultimediaModel> Medias { get; set; }

    //public IEnumerable<CommentModel> Replies { get; set; }               // FE Load if explicitly called
    //...
    public IEnumerable<ReactionModel> Reactions { get; set; }






    public static Func<Comment, CommentModel> MapFunc
        = _ => new CommentModel
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
        };

    public static Expression<Func<Comment, CommentModel>> MapExpression
        = _ => new CommentModel
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
        };
}
