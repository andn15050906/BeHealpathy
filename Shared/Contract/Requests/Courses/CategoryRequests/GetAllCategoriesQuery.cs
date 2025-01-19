using Contract.Responses.Courses;

namespace Contract.Requests.Courses.CategoryRequests;

public sealed class GetAllCategoriesQuery : IRequest<Result<List<CategoryModel>>>
{
}