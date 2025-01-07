using Contract.Requests.Payment.Dtos;

namespace Contract.Requests.Payment;

public sealed class CreateBillCommand : CreateCommand
{
    public CreateBillDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateBillCommand(Guid id, CreateBillDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
