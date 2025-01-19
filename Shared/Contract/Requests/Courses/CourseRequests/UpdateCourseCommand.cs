using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Courses.CourseRequests.Dtos;

namespace Contract.Requests.Courses.CourseRequests;

public sealed class UpdateCourseCommand : UpdateCommand
{
    public UpdateCourseDto Rq { get; init; }
    public Guid UserId { get; init; }
    public Guid InstructorId { get; init; }
    public List<Multimedia>? AddedMedias { get; init; }
    public List<Guid>? RemovedMedias { get; init; }



    public UpdateCourseCommand(
        UpdateCourseDto rq, Guid userId, Guid instructorId,
        List<Multimedia>? addedMedias, List<Guid>? removedMedias, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
        InstructorId = instructorId;
        AddedMedias = addedMedias;
        RemovedMedias = removedMedias;
    }
}