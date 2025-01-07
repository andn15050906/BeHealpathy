namespace Contract.Domain.Shared;

public sealed class Bill : CreationAuditedEntity
{
    // Attributes
    public string Action { get; set; }                      // System Action
    public string Note { get; set; }
    public long Amount { get; set; }                        // Not double
    public string Gateway { get; set; }
    public string TransactionId { get; set; }
    public string ClientTransactionId { get; set; }
    public string Token { get; set; }
    public bool IsSuccessful { get; set; }






#pragma warning disable CS8618
    public Bill()
    {

    }
#pragma warning restore CS8618

    public Bill(Guid id, Guid creatorId, string action, string note, long amount, string gateway, string transactionId, string clientTransactionId, string token, bool isSuccessful)
    {
        Id = id;
        CreatorId = creatorId;

        Action = action;
        Note = note;
        Amount = amount;
        Gateway = gateway;
        TransactionId = transactionId;
        ClientTransactionId = clientTransactionId;
        Token = token;
        IsSuccessful = isSuccessful;
    }
}