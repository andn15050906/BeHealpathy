using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Shared.BaseDtos.Comments;

namespace Contract.Requests.Shared.BaseRequests.Comments;

public class CreateCommentCommand : CreateCommand
{
    public CreateCommentDto Rq { get; init; }
    public Guid UserId { get; init; }
    public List<Multimedia>? Medias { get; init; }



    public CreateCommentCommand(Guid id, CreateCommentDto rq, Guid userId, List<Multimedia>? medias, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
        Medias = medias;
    }
}
