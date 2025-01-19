using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Library.ArticleRequests.Dtos;

public sealed class CreateArticleDto
{
    public string Title { get; set; }
    public string Status { get; set; }
    public bool IsCommentDisabled { get; set; }

    public List<Guid> Tags { get; set; }
    public CreateMediaDto Thumb { get; set; }
    public List<CreateArticleSectionDto> Sections { get; set; }
}
