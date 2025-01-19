namespace Contract.Requests.Progress.SubmissionRequests;

public sealed class DeleteSubmissionCommand : DeleteCommand
{
    public DeleteSubmissionCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}