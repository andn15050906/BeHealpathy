using Contract.Requests.Shared.Base;
using Contract.Responses.Identity.UserModels;

namespace Contract.Requests.Identity;

public sealed class GetUserByIdQuery : GetByIdQuery<UserFullModel>
{
    public GetUserByIdQuery(Guid id) : base(id)
    {
    }
}
