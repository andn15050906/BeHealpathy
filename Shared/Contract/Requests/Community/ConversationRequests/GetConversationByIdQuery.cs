using Contract.Responses.Community;

namespace Contract.Requests.Community.ConversationRequests;

public sealed class GetConversationByIdQuery : GetByIdQuery<ConversationModel>
{
    public GetConversationByIdQuery(Guid id) : base(id)
    {
    }
}