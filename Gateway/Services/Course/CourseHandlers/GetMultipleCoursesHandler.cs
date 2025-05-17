using Contract.Helpers;
using Contract.Requests.Courses.CourseRequests;
using Contract.Responses.Courses;
using Microsoft.EntityFrameworkCore;

namespace Courses.Services.Courses;

public sealed class GetMultipleCoursesHandler : RequestHandler<GetMultipleCoursesQuery, List<CourseModel>, HealpathyContext>
{
    public GetMultipleCoursesHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<List<CourseModel>>> Handle(GetMultipleCoursesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _context.Courses
                .Where(_ => request.Ids.Contains(_.Id))
                .Select(CourseModel.MapExpression)
                .ToListAsync(cancellationToken);

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    /*public override async Task<Result<List<CourseOverviewModel>>> Handle(GetMultipleCoursesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _context.Find(
                Builders<Course>.Filter.In(_ => _.Id, request.Ids),
                Builders<Course>.Projection.Expression(CourseOverviewModel.MapExpression)
            );

            return Ok(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }*/
}
