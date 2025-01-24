using Contract.Requests.Shared.BaseDtos.Reactions;
using Contract.Responses.Shared;

namespace Contract.Requests.Shared.BaseRequests.Reactions;

public class CreateReactionCommand : CreateCommand<ReactionModel>
{
    public CreateReactionDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateReactionCommand(Guid id, CreateReactionDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
