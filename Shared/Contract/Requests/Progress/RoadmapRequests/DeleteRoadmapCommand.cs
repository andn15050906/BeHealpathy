namespace Contract.Requests.Progress.RoadmapRequests;

public sealed class DeleteRoadmapCommand : DeleteCommand
{
    public DeleteRoadmapCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}