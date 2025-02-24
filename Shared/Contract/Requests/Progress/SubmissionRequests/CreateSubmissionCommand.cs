using Contract.Requests.Progress.SubmissionRequests.Dtos;

namespace Contract.Requests.Progress.SubmissionRequests;

public sealed class CreateSubmissionCommand : CreateCommand<Guid>
{
    public CreateSubmissionDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateSubmissionCommand(Guid id, CreateSubmissionDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
