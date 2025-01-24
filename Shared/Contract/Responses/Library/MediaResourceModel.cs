using Contract.Domain.LibraryAggregate;
using Contract.Domain.LibraryAggregate.Enums;
using Contract.Responses.Shared;
using System.Linq.Expressions;

namespace Contract.Responses.Library;

public sealed class MediaResourceModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Description { get; set; }
    public string Artist { get; set; }
    public string Title { get; set; }
    public MediaResourceType Type { get; set; }

    public MultimediaModel Media { get; set; }






    public static Func<MediaResource, MediaResourceModel> MapFunc
        = _ => new MediaResourceModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            LastModifierId = _.LastModifierId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Description = _.Description,
            Artist = _.Artist,
            Title = _.Title,
            Type = _.Type,

            //Media
        };

    public static Expression<Func<MediaResource, MediaResourceModel>> MapExpression
        = _ => new MediaResourceModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            LastModifierId = _.LastModifierId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Description = _.Description,
            Artist = _.Artist,
            Title = _.Title,
            Type = _.Type,

            //Media
        };
}