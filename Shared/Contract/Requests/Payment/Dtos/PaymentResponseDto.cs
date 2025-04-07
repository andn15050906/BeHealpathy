namespace Contract.Requests.Payment.Dtos;

public sealed class PaymentResponseDto
{
    public string code { get; set; }
    public string id { get; set; }
    public bool cancel { get; set; }
    public string status { get; set; }
    public string orderCode { get; set; }

    /*public long vnp_Amount { get; set; }
    public string vnp_BankCode { get; set; }
    public string? vnp_BankTranNo { get; set; }
    public string vnp_CardType { get; set; }
    public string vnp_OrderInfo { get; set; }
    public string vnp_PayDate { get; set; }
    public string vnp_ResponseCode { get; set; }
    public string vnp_TmnCode { get; set; }
    public string vnp_TransactionNo { get; set; }
    public string vnp_TransactionStatus { get; set; }
    public string vnp_TxnRef { get; set; }
    public string vnp_SecureHash { get; set; }*/
}
