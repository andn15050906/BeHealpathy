namespace Contract.Requests.Payment.Dtos;

public sealed class UpdateBillDto
{
    public Guid Id { get; set; }

    // Filled by client
    public string? Action { get; set; }                  // PaymentDomainMessage
    //public string Gateway { get; set; }

    // Filled by gateway
    public string? Note { get; set; }                    // TargetId
    public long? Amount { get; set; }
    public string? TransactionId { get; set; }
    public string? ClientTransactionId { get; set; }
    public string? Token { get; set; }
    public bool? IsSuccessful { get; set; }
}