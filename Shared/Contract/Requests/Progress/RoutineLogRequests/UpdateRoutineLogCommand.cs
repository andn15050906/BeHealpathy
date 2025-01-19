using Contract.Requests.Progress.RoutineLogRequests.Dtos;

namespace Contract.Requests.Progress.RoutineLogRequests;

public sealed class UpdateRoutineLogCommand : UpdateCommand
{
    public UpdateRoutineLogDto Rq { get; init; }
    public Guid UserId { get; init; }



    public UpdateRoutineLogCommand(UpdateRoutineLogDto rq, Guid userId, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}