using Contract.Requests.Community.MeetingRequests.Dtos;

namespace Contract.Requests.Community.MeetingRequests;

public sealed class CreateMeetingCommand : CreateCommand
{
    public CreateMeetingDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateMeetingCommand(Guid id, CreateMeetingDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
