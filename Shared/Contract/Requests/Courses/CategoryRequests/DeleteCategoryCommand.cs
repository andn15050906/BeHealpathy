namespace Contract.Requests.Courses.CategoryRequests;

public sealed class DeleteCategoryCommand : DeleteCommand
{
    public DeleteCategoryCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    { }
}
