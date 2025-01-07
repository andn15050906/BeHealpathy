namespace Contract.Requests.Library.MediaRequests;

public sealed class DeleteMediaResourceCommand : DeleteCommand
{
    public DeleteMediaResourceCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}