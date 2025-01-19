using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.CategoryRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.CategoryHandlers;

public sealed class CreateCategoryHandler : RequestHandler<CreateCategoryCommand, HealpathyContext>
{
    public CreateCategoryHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Category? parent = null;
            if (request.Rq.ParentId is not null)
            {
                parent = await _context.Categories.FindExt(request.Rq.ParentId);
                if (parent is null)
                    return BadRequest(BusinessMessages.Category.INVALID_PARENT);
            }

            var entity = Adapt(request, parent);
            await _context.Categories.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Created();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private static Category Adapt(CreateCategoryCommand command, Category? parent)
    {
        return new Category(command.Id, command.Rq.Title, command.Rq.Description, command.Rq.IsLeaf, parent);
    }
}
