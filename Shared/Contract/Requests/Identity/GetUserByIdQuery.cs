using Contract.Requests.Shared.Base;
using Contract.Responses.Identity;

namespace Contract.Requests.Identity;

public sealed class GetUserByIdQuery : GetByIdQuery<UserFullModel>
{
    public GetUserByIdQuery(Guid id) : base(id)
    {
    }
}
