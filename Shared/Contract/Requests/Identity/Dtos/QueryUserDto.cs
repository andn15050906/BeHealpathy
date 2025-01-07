using Contract.Requests.Shared.Base;

namespace Contract.Requests.Identity.Dtos;

public sealed class QueryUserDto : PagingQueryDto
{
    public string? Name { get; set; }       // suggest
}
