using Contract.Responses.Library;

namespace Contract.Requests.Library.ArticleRequests;

public sealed class GetArticleByIdQuery : GetByIdQuery<ArticleModel>
{
    public GetArticleByIdQuery(Guid id) : base(id)
    {
    }
}