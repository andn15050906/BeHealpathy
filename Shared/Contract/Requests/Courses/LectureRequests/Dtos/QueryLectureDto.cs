namespace Contract.Requests.Courses.LectureRequests.Dtos;

public sealed class QueryLectureDto : PagingQueryDto
{
    public Guid? Id { get; set; }                   // GetLectureById
    public Guid? CourseId { get; set; }
    //public Guid? SectionId { get; set; }
}
