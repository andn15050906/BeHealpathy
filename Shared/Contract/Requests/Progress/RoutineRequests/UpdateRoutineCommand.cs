using Contract.Requests.Progress.RoutineRequests.Dtos;

namespace Contract.Requests.Progress.RoutineRequests;

public sealed class UpdateRoutineCommand : UpdateCommand
{
    public UpdateRoutineDto Rq { get; init; }
    public Guid UserId { get; init; }



    public UpdateRoutineCommand(UpdateRoutineDto rq, Guid userId, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}