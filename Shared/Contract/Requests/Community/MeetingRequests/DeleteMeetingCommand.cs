namespace Contract.Requests.Community.MeetingRequests;

public sealed class DeleteMeetingCommand : DeleteCommand
{
    public DeleteMeetingCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}