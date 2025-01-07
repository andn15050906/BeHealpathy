using Contract.Requests.Progress.RoutineLogRequests.Dtos;

namespace Contract.Requests.Progress.RoutineLogRequests;

public sealed class CreateRoutineLogCommand : CreateCommand
{
    public CreateRoutineLogDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateRoutineLogCommand(Guid id, CreateRoutineLogDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
