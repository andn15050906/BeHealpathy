namespace Contract.Requests.Progress.RoutineLogRequests;

public sealed class DeleteRoutineLogCommand : DeleteCommand
{
    public DeleteRoutineLogCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}