using Contract.Domain.ToolAggregate;
using System.Linq.Expressions;

namespace Contract.Responses.Library;

public sealed class TagModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }



    public static Func<Tag, TagModel> MapFunc
        = _ => new TagModel
        {
            Id = _.Id,
            Title = _.Title
        };

    public static Expression<Func<Tag, TagModel>> MapExpression
        = _ => new TagModel
        {
            Id = _.Id,
            Title = _.Title
        };
}
