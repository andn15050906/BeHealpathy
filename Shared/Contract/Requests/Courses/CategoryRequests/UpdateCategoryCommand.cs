using Contract.Requests.Courses.CategoryRequests.Dtos;

namespace Contract.Requests.Courses.CategoryRequests;

public sealed class UpdateCategoryCommand : UpdateCommand
{
    public UpdateCategoryDto Rq { get; init; }
    public Guid UserId { get; init; }



    public UpdateCategoryCommand(UpdateCategoryDto rq, Guid userId, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
