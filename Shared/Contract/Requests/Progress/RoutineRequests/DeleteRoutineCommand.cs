namespace Contract.Requests.Progress.RoutineRequests;

public sealed class DeleteRoutineCommand : DeleteCommand
{
    public DeleteRoutineCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}