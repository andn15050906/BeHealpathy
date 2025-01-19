using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Courses.LectureRequests.Dtos;

namespace Contract.Requests.Courses.LectureRequests;

public sealed class UpdateLectureCommand : UpdateCommand
{
    public UpdateLectureDto Rq { get; init; }
    public Guid UserId { get; init; }
    public Guid InstructorId { get; init; }
    public List<Multimedia> AddedMedias { get; init; }
    public List<Guid> RemovedMedias { get; init; }



    public UpdateLectureCommand(
        UpdateLectureDto rq, Guid userId, Guid instructorId,
        List<Multimedia> addedMedias, List<Guid> removedMedias, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
        InstructorId = instructorId;
        AddedMedias = addedMedias;
        RemovedMedias = removedMedias;
    }
}