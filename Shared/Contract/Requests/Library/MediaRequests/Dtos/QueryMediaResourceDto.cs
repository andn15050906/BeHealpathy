using Contract.Domain.LibraryAggregate.Enums;

namespace Contract.Requests.Library.MediaRequests.Dtos;

public sealed class QueryMediaResourceDto : PagingQueryDto
{
    public Guid? CreatorId { get; set; }

    public string? Description { get; set; }
    public string? Artist { get; set; }
    public string? Title { get; set; }
    public MediaResourceType? Type { get; set; }
}