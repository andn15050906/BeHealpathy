using Contract.Requests.Courses.CategoryRequests.Dtos;
using Contract.Requests.Courses.CategoryRequests;
using Contract.Helpers;
using Infrastructure.DataAccess.SQLServer.Helpers;
using Contract.Domain.CourseAggregate;

namespace Gateway.Services.Course.CategoryHandlers;

public sealed class UpdateCategoryHandler : RequestHandler<UpdateCategoryCommand, HealpathyContext>
{
    public UpdateCategoryHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _context.Categories.FindExt(request.Rq.Id);
            if (entity is null)
                return BadRequest(BusinessMessages.Category.INVALID_ID);

            Category? parent = null;
            if (request.Rq.ParentId is not null)
            {
                parent = await _context.Categories.FindExt(request.Rq.ParentId);
                if (parent is null)
                    return BadRequest(BusinessMessages.Category.INVALID_PARENT);
            }

            ApplyChanges(request.Rq, entity, parent);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private static void ApplyChanges(UpdateCategoryDto dto, Category entity, Category? parent)
    {
        if (dto.Title is not null)
            entity.Title = dto.Title;
        if (dto.Description is not null)
            entity.Description = dto.Description;

        if (parent is not null)
            entity.SetPath(parent);
    }
}
