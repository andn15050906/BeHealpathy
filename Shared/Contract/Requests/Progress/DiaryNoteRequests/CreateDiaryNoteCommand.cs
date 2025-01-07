using Contract.Requests.Progress.DiaryNoteRequests.Dtos;

namespace Contract.Requests.Progress.DiaryNoteRequests;

public sealed class CreateDiaryNoteCommand : CreateCommand
{
    public CreateDiaryNoteDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateDiaryNoteCommand(Guid id, CreateDiaryNoteDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
