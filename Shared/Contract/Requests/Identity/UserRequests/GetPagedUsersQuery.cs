using Contract.Requests.Identity.UserRequests.Dtos;
using Contract.Responses.Identity;

namespace Contract.Requests.Identity.UserRequests;

public sealed class GetPagedUsersQuery : IRequest<Result<PagedResult<UserOverviewModel>>>
{
    public QueryUserDto Rq { get; init; }

    public GetPagedUsersQuery(QueryUserDto dto)
    {
        Rq = dto;
    }
}
