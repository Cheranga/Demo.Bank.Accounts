namespace Demo.Bank.Accounts.Api.Features.TransferMoney;

public class TransferMoneyRequest
{
    public string CorrelationId { get; set; }
    public string CustomerId { get; set; }
    public string FromAccount { get; set; }
    public string ToAccount { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransferredAt { get; set; }
    
    public TransferMoneyRequest()
    {
        CorrelationId = string.Empty;
        CustomerId = string.Empty;
        FromAccount = string.Empty;
        ToAccount = string.Empty;
        Amount = 0;
    }
}