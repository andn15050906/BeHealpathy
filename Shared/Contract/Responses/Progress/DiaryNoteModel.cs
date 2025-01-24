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
    public string Title { get; set; }
    public string Content { get; set; }
    public string Mood { get; set; }
    public string Theme { get; set; }
    public IEnumerable<MultimediaModel> Medias { get; set; }






    public static Func<DiaryNote, DiaryNoteModel> MapFunc
        = _ => new DiaryNoteModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,
            Title = _.Title,
            Content = _.Content,
            Mood = _.Mood,
            Theme = _.Theme,
            //Medias
        };

    public static Expression<Func<DiaryNote, DiaryNoteModel>> MapExpression
        = _ => new DiaryNoteModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,
            Title = _.Title,
            Content = _.Content,
            Mood = _.Mood,
            Theme = _.Theme,
            //Medias
        };
}