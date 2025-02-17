using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Shared.BaseDtos.Comments;
using Contract.Responses.Shared;

namespace Contract.Requests.Shared.BaseRequests.Comments;

public class CreateCommentCommand : CreateCommand<CommentModel>
{
    public CreateCommentDto Rq { get; init; }
    public Guid UserId { get; init; }
    public List<Multimedia>? Medias { get; set; }



    public CreateCommentCommand(Guid id, CreateCommentDto rq, Guid userId, List<Multimedia>? medias, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
        Medias = medias;
    }
}
