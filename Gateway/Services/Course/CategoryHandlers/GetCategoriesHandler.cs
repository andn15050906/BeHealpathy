using Contract.Helpers;
using Contract.Requests.Courses.CategoryRequests;
using Contract.Responses.Courses;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Course.CategoryHandlers;

public sealed class GetAllCategoriesHandler : RequestHandler<GetAllCategoriesQuery, List<CategoryModel>, HealpathyContext>
{
    public GetAllCategoriesHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result<List<CategoryModel>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _context.Categories.Select(CategoryModel.MapExpression).ToListAsync();
            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}
