namespace Contract.Requests.Notifications.Dtos;

public sealed class CreateWithdrawalRequestDto
{
    public string Bank { get; set; }
    public string AccountNumber { get; set; }
    public string AccountName { get; set; }
    public int Amount { get; set; }
}