using Contract.Domain.Shared.ReviewBase;
using System.Linq.Expressions;

namespace Contract.Responses.Shared;

public class ReviewModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public Guid SourceId { get; set; }

    public string Content { get; set; }
    public byte Rating { get; set; }
    public IEnumerable<MultimediaModel> Medias { get; set; }






    public static Func<Review, ReviewModel> MapFunc
        = _ => new ReviewModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            LastModifierId = _.LastModifierId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Content = _.Content,
            Rating = _.Rating,
            //Medias = _.Medias,

            SourceId = _.SourceId
        };

    public static Expression<Func<Review, ReviewModel>> MapExpression
        = _ => new ReviewModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            LastModifierId = _.LastModifierId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Content = _.Content,
            Rating = _.Rating,
            //Medias = _.Medias,

            SourceId = _.SourceId
        };
}
