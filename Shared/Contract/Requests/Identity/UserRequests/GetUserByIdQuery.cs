using Contract.Responses.Identity;

namespace Contract.Requests.Identity.UserRequests;

public sealed class GetUserByIdQuery : GetByIdQuery<UserFullModel>
{
    public GetUserByIdQuery(Guid id) : base(id)
    {
    }
}
