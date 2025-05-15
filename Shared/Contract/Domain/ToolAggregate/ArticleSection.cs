using Contract.Domain.Shared.MultimediaBase;

namespace Contract.Domain.ToolAggregate;

//Need Id for quick navigation
public sealed class ArticleSection : Entity
{
    public Guid ArticleId { get; set; }

    public string Header { get; set; }
    public string Content { get; set; }

    public Multimedia Media { get; set; }
    public Article Article { get; set; }

#pragma warning disable CS8618
    public ArticleSection()
    {

    }

    public ArticleSection(Guid id, Guid articleId, string header, string content)
    {
        Id = id;
        ArticleId = articleId;
        Header = header;
        Content = content;
    }
#pragma warning restore CS8618
}
