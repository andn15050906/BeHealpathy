using Contract.Requests.Community.MeetingRequests.Dtos;

namespace Contract.Requests.Community.MeetingRequests;

public sealed class UpdateMeetingCommand : UpdateCommand
{
    public UpdateMeetingDto Rq { get; init; }
    public Guid UserId { get; init; }



    public UpdateMeetingCommand(UpdateMeetingDto rq, Guid userId, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}