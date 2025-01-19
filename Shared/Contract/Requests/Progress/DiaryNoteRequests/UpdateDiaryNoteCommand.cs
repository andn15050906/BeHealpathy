using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Progress.DiaryNoteRequests.Dtos;

namespace Contract.Requests.Progress.DiaryNoteRequests;

public sealed class UpdateDiaryNoteCommand : UpdateCommand
{
    public UpdateDiaryNoteDto Rq { get; init; }
    public Guid UserId { get; init; }
    public List<Multimedia> AddedMedias { get; init; }
    public List<Guid> RemovedMedias { get; init; }



    public UpdateDiaryNoteCommand(UpdateDiaryNoteDto rq, Guid userId, List<Multimedia> addedMedias, List<Guid> removedMedias, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
        AddedMedias = addedMedias;
        RemovedMedias = removedMedias;
    }
}