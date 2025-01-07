using Contract.Messaging.Models;
using Contract.Requests.Identity.Dtos;
using Contract.Responses.Identity.UserModels;

namespace Contract.Requests.Identity;

public sealed class GetPagedUsersQuery : IRequest<Result<PagedResult<UserOverviewModel>>>
{
    public QueryUserDto Rq { get; init; }

    public GetPagedUsersQuery(QueryUserDto dto)
    {
        Rq = dto;
    }
}
