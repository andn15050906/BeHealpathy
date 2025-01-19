namespace Contract.Requests.Library.TagRequests;

public sealed class DeleteTagCommand : DeleteCommand
{
    public DeleteTagCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}