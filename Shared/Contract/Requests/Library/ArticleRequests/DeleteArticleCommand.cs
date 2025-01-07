namespace Contract.Requests.Community.ArticleRequests;

public sealed class DeleteArticleCommand : DeleteCommand
{
    public DeleteArticleCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}