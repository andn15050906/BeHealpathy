namespace Contract.Domain.ToolAggregate;

public sealed class ArticleTag : DomainObject
{
    public Guid ArticleId { get; set; }
    public Guid TagId { get; set; }
}
