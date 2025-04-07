namespace Contract.Requests.Payment.Dtos;

public sealed class CreateBillDto
{
    // Filled by client
    public string Action { get; set; }              = string.Empty; // PaymentDomainMessage
    public string Gateway { get; set; }             = string.Empty;

    // Filled by gateway
    public string Note { get; set; }                = string.Empty; // TargetId
    public long Amount { get; set; }
    public string TransactionId { get; set; }       = string.Empty;
    public string ClientTransactionId { get; set; } = string.Empty;
    public string Token { get; set; }               = string.Empty;
    public bool IsSuccessful { get; set; }
}
