namespace Contract.Requests.Progress.McqRequests;

public sealed class DeleteMcqQuestionCommand : DeleteCommand
{
    public DeleteMcqQuestionCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}