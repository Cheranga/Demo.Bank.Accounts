using Demo.Bank.Accounts.Api.Features.Shared;
using Demo.Bank.Accounts.Api.Shared;

namespace Demo.Bank.Accounts.Api.Features.CreateBankAccount;

public class CreateBankAccountRequest : IIdentifier
{
    public CreateBankAccountRequest()
    {
        CorrelationId = string.Empty;
        ClientId = string.Empty;
        Name = string.Empty;
        Address = string.Empty;
        BankAccountId = string.Empty;
        OpeningBalance = 0;
    }

    public string BankAccountId { get; set; }
    public string CorrelationId { get; set; }
    public string ClientId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public decimal OpeningBalance { get; set; }
}