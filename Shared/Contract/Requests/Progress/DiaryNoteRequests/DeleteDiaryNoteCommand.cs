namespace Contract.Requests.Progress.DiaryNoteRequests;

public sealed class DeleteDiaryNoteCommand : DeleteCommand
{
    public DeleteDiaryNoteCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}