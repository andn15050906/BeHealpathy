using Contract.Domain.UserAggregate.Enums;

namespace Contract.Requests.Identity.UserRequests.Dtos;

public sealed class QueryUserDto : PagingQueryDto
{
    public string? Name { get; set; }       // suggest
    public Guid? AdvisorId { get; set; }

    // Authorized only
    public string? Email { get; set; }
    public Role? Role { get; set; }
    public bool? IsVerified { get; set; }
    public bool? IsApproved { get; set; }
    public bool? IsBanned { get; set; }
}
