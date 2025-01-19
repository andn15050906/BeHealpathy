using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Library.ArticleRequests.Dtos;

public sealed class UpdateArticleSectionDto
{
    public Guid Id { get; set; }
    public string? Header { get; set; }
    public string? Content { get; set; }

    public CreateMediaDto? AddedMedia { get; set; }
    public Guid RemovedMedia { get; set; }
}