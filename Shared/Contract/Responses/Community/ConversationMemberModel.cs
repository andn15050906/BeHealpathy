using Contract.Domain.CommunityAggregate;
using System.Linq.Expressions;

namespace Contract.Responses.Community;

public sealed class ConversationMemberModel
{
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }

    public Guid ConversationId { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime LastVisit { get; set; }



    public static Expression<Func<ConversationMember, ConversationMemberModel>> MapExpression
        = _ => new ConversationMemberModel
        {
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,
            ConversationId = _.ConversationId,
            IsAdmin = _.IsAdmin,
            LastVisit = _.LastVisit
        };
}
