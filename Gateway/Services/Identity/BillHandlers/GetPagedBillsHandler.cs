using Contract.Helpers;
using System.Linq.Expressions;
using Contract.Responses.Payment;
using Contract.Requests.Payment;
using Contract.Domain.Shared;
using Contract.Requests.Payment.Dtos;

namespace Gateway.Services.Identity.BillHandlers;

public class GetPagedBillsHandler : RequestHandler<GetPagedBillsQuery, PagedResult<BillModel>, HealpathyContext>
{
    public GetPagedBillsHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<PagedResult<BillModel>>> Handle(GetPagedBillsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                BillModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                1024,//request.Rq.PageSize,
                false
            );
            var result = await query.ExecuteWithOrderBy(_ => _.CreationTime, ascending: false);
            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<Bill, bool>>? GetPredicate(QueryBillDto dto)
    {
        return null;
    }
}