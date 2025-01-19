using Contract.Requests.Courses.CategoryRequests.Dtos;

namespace Contract.Requests.Courses.CategoryRequests;

public sealed class CreateCategoryCommand : CreateCommand
{
    public CreateCategoryDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateCategoryCommand(Guid id, CreateCategoryDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
