using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Courses.LectureRequests.Dtos;

namespace Contract.Requests.Courses.LectureRequests;

public sealed class CreateLectureCommand : CreateCommand
{
    public CreateLectureDto Rq { get; init; }
    public Guid UserId { get; init; }
    public Guid InstructorId { get; init; }
    public List<Multimedia> Medias { get; init; }



    public CreateLectureCommand(
        Guid id, CreateLectureDto rq, Guid userId, Guid instructorId,
        List<Multimedia> medias, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
        InstructorId = instructorId;
        Medias = medias;
    }
}