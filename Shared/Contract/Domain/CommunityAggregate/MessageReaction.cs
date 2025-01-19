using Contract.Domain.Shared.ReactionBase;

namespace Contract.Domain.CommunityAggregate;

public sealed class MessageReaction : Reaction
{
    public MessageReaction() : base()
    {

    }

    public MessageReaction(Guid id, Guid creatorId, Guid sourceEntityId, string content) : base(id, creatorId, sourceEntityId, content)
    {

    }
}
