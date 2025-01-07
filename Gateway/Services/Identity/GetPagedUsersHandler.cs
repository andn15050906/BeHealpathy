using Contract.Helpers;
using Contract.Requests.Identity;
using Contract.Requests.Identity.Dtos;
using Contract.Responses.Identity.UserModels;
using Core.Helpers;
using System.Linq.Expressions;

namespace Gateway.Services.Identity;

public sealed class GetPagedUsersHandler : RequestHandler<GetPagedUsersQuery, PagedResult<UserOverviewModel>, HealpathyContext>
{
    public GetPagedUsersHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<PagedResult<UserOverviewModel>>> Handle(GetPagedUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                UserOverviewModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize
            );

            var result = await query.ExecuteWithOrderBy(_ => _.MetaFullName);

            /*var result = await _context.FindPaged(
                Builders<User>.Filter.Empty,
                Builders<User>.Projection.Expression(UserOverviewModel.MapExpression),
                _ => _.UserName, true, request.Rq.PageIndex, request.Rq.PageSize);

            return Ok(result);*/
            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<User, bool>>? GetPredicate(QueryUserDto dto)
    {
        if (dto.Name is not null)
            return _ => _.MetaFullName.Contains(TextHelper.Normalize(dto.Name));
        return null;
    }
}
