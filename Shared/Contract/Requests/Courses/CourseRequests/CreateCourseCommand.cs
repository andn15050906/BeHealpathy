using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Courses.CourseRequests.Dtos;

namespace Contract.Requests.Courses.CourseRequests;

public sealed class CreateCourseCommand : CreateCommand
{
    public CreateCourseDto Rq { get; init; }
    public Guid UserId { get; init; }
    public Guid InstructorId { get; init; }
    public List<Multimedia> Medias { get; init; }



    public CreateCourseCommand(
        Guid id, CreateCourseDto rq, Guid userId, Guid instructorId,
        List<Multimedia> medias, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
        InstructorId = instructorId;
        Medias = medias;
    }
}