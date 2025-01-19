using Contract.Requests.Progress.McqRequests.Dtos;

namespace Contract.Requests.Progress.McqRequests;

public sealed class CreateMcqQuestionCommand : CreateCommand
{
    public CreateMcqQuestionDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateMcqQuestionCommand(Guid id, CreateMcqQuestionDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
