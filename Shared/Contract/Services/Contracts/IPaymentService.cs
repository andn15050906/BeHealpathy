using Contract.Requests.Payment.Dtos;
using Contract.Responses.Payment;
using Core.Responses;

namespace Contract.Services.Contracts;

public interface IPaymentService
{
    Task<PagedResult<BillModel>> GetPagedAsync(QueryBillDto dto);

    Task<string> GetUrl(CreateBillDto dto);
}