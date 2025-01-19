using Contract.Domain.Shared.ReactionBase;
using System.Linq.Expressions;

namespace Contract.Responses.Shared;

public class ReactionModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }

    public Guid SourceId { get; set; }

    public string Content { get; set; }






    public static Func<Reaction, ReactionModel> MapFunc
        = _ => new ReactionModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,

            SourceId = _.SourceId,

            Content = _.Content
        };

    public static Expression<Func<Reaction, ReactionModel>> MapExpression
        = _ => new ReactionModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,

            SourceId = _.SourceId,

            Content = _.Content
        };
}
