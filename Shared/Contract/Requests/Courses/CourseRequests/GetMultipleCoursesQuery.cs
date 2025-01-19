using Contract.Responses.Courses;

namespace Contract.Requests.Courses.CourseRequests;

public sealed class GetMultipleCoursesQuery : IRequest<Result<List<CourseOverviewModel>>>
{
    public IEnumerable<Guid> Ids { get; set; }



    public GetMultipleCoursesQuery(IEnumerable<Guid> ids)
    {
        Ids = ids;
    }
}