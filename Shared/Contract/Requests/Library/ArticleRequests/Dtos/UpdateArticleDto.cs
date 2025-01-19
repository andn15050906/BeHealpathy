using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Library.ArticleRequests.Dtos;

public sealed class UpdateArticleDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; }
    public string? Status { get; set; }
    public bool? IsCommentDisabled { get; set; }

    public List<Guid>? RemovedTags { get; set; }
    public List<Guid>? AddedTags { get; set; }
    public CreateMediaDto? Thumb { get; set; }
    public List<UpdateArticleSectionDto>? Sections { get; set; }
}
