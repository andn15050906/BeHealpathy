using Contract.Messaging.Models;
using Contract.Requests.Payment.Dtos;
using Contract.Responses.Payment;

namespace Contract.Requests.Payment;

public sealed class GetPagedBillsQuery : IRequest<Result<PagedResult<BillModel>>>
{
    public QueryBillDto Rq { get; init; }



    public GetPagedBillsQuery(QueryBillDto rq)
    {
        Rq = rq;
    }
}
