using Demo.Bank.Accounts.Api.Infrastructure.DataAccess;

namespace Demo.Bank.Accounts.Api.Features.SaveBankAccount;

public class SaveBankAccountCommand : ICommand
{
    public string BankAccountId { get; set; }
    public string CorrelationId { get; set; }
    public string ClientId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public decimal OpeningBalance { get; set; }
    public DateTime CreatedAt { get; set; }

    public SaveBankAccountCommand()
    {
        CorrelationId = string.Empty;
        ClientId = string.Empty;
        Name = string.Empty;
        Address = string.Empty;
        BankAccountId = string.Empty;
        OpeningBalance = 0;
    }
}