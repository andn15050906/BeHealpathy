using Contract.Domain.ProgressAggregates;
using Contract.Responses.Shared;
using System.Linq.Expressions;

namespace Contract.Responses.Progress;

public sealed class DiaryNoteModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
    public List<MultimediaModel> Medias { get; set; }



    public static Expression<Func<DiaryNote, DiaryNoteModel>> MapExpression
        = _ => new DiaryNoteModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,
            //Medias
        };
}