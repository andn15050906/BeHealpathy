using Contract.Requests.Payment.Dtos;

namespace Contract.Requests.Payment;

public sealed class UpdateBillCommand : UpdateCommand<Guid>
{
    public UpdateBillDto Rq { get; init; }
    public Guid UserId { get; init; }



    public UpdateBillCommand(UpdateBillDto rq, Guid userId, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}