using Contract.Responses.Library;

namespace Contract.Requests.Library.TagRequests;

public sealed class GetAllTagsQuery : IRequest<Result<List<TagModel>>>
{
}