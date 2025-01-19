using Contract.Domain.Shared.MultimediaBase;
using Contract.Domain.Shared.MultimediaBase.Enums;
using System.Linq.Expressions;

namespace Contract.Responses.Shared;

public sealed class MultimediaModel
{
    public Guid Id { get; set; }
    public Guid SourceId { get; set; }
    public MediaType Type { get; init; }
    public string Url { get; set; }
    public string Title { get; init; }



    public static Func<Multimedia, MultimediaModel> MapFunc
        = _ => new MultimediaModel
        {
            Id = _.Id,

            SourceId = _.SourceId,
            Type = _.Type,
            Url = _.Url,
            Title = _.Title
        };

    public static Expression<Func<Multimedia, MultimediaModel>> MapExpression
        = _ => new MultimediaModel
        {
            Id = _.Id,

            SourceId = _.SourceId,
            Type = _.Type,
            Url = _.Url,
            Title = _.Title
        };
}
