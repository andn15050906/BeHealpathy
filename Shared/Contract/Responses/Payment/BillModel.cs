using Contract.Domain.Shared;
using System.Linq.Expressions;

namespace Contract.Responses.Payment;

public sealed class BillModel
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid CreatorId { get; set; }

    public string Action { get; set; }                      // System Action
    public string Note { get; set; }
    public long Amount { get; set; }                        // Not double
    public string Gateway { get; set; }
    public string TransactionId { get; set; }
    public string ClientTransactionId { get; set; }
    public string Token { get; set; }
    //public bool IsSuccessful { get; set; }



    public static Expression<Func<Bill, BillModel>> MapExpression
        => _ => new BillModel
        {
            Id = _.Id,
            CreationTime = _.CreationTime,
            CreatorId = _.CreatorId,
            Action = _.Action,
            Note = _.Note,
            Amount = _.Amount,
            Gateway = _.Gateway,
            TransactionId = _.TransactionId,
            ClientTransactionId = _.ClientTransactionId,
            Token = _.Token,
            //IsSuccessful = _.IsSuccessful
        };
}
