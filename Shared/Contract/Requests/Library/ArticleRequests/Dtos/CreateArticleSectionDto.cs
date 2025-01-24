using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Library.ArticleRequests.Dtos;

public sealed class CreateArticleSectionDto
{
    public Guid Id { get; set; }            // not set from Client

    public string Header { get; set; }
    public string Content { get; set; }

    public CreateMediaDto? Media { get; set; }
}
