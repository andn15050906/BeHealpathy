using Contract.Helpers;
using Contract.Requests.Courses.CourseRequests;
using Contract.Responses.Courses;
using Microsoft.EntityFrameworkCore;

namespace Courses.Services.Courses;

public class GetCourseByIdHandler : RequestHandler<GetCourseByIdQuery, CourseModel, HealpathyContext>
{
    public GetCourseByIdHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<CourseModel>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _context.Courses
                .Where(_ => _.Id == request.Id)
                .Select(CourseModel.MapExpression)
                .FirstOrDefaultAsync(cancellationToken);

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}
