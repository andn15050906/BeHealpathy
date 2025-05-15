using Contract.Domain.ToolAggregate;
using Contract.Responses.Shared;
using System.Linq.Expressions;

namespace Contract.Responses.Library;

public sealed class ArticleSectionModel
{
    public Guid Id { get; set; }
    public string Header { get; set; }
    public string Content { get; set; }
    public MultimediaModel? Media { get; set; }



    public static Expression<Func<ArticleSection, ArticleSectionModel>> MapExpression
        = _ => new ArticleSectionModel
        {
            Id = _.Id,
            Header = _.Header,
            Content = _.Content,
            //Media
        };
}
