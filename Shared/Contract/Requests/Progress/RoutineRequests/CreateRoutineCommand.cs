using Contract.Requests.Progress.RoutineRequests.Dtos;
using Contract.Responses.Progress;

namespace Contract.Requests.Progress.RoutineRequests;

public sealed class CreateRoutineCommand : CreateCommand<RoutineModel>
{
    public CreateRoutineDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateRoutineCommand(Guid id, CreateRoutineDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}