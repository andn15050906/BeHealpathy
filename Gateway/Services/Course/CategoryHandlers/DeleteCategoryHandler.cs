using Contract.Helpers;
using Contract.Requests.Courses.CategoryRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.CategoryHandlers;

public sealed class DeleteCategoryHandler : RequestHandler<DeleteCategoryCommand, HealpathyContext>
{
    public DeleteCategoryHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _context.Categories.DeleteExt(request.Id, cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}
