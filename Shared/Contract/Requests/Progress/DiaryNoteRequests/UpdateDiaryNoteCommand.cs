using Contract.Requests.Progress.DiaryNoteRequests.Dtos;

namespace Contract.Requests.Progress.DiaryNoteRequests;

public sealed class UpdateDiaryNoteCommand : UpdateCommand
{
    public UpdateDiaryNoteDto Rq { get; init; }
    public Guid UserId { get; init; }



    public UpdateDiaryNoteCommand(UpdateDiaryNoteDto rq, Guid userId, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}