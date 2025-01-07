using Contract.Domain.Shared.CommentBase;

namespace Contract.Domain.CommunityAggregate;

public sealed class MessageReaction : Reaction
{
    public MessageReaction()
    {

    }

    public MessageReaction(Guid creatorId, Guid sourceEntityId, string content) : base(creatorId, sourceEntityId, content)
    {

    }
}
