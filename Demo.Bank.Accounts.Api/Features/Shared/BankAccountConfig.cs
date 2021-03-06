namespace Demo.Bank.Accounts.Api.Features.Shared;

public class BankAccountConfig
{
    public string NewBankAccountsQueue { get; set; }
    public string DatabaseConnectionString { get; set; }

    public int VisibilityInSeconds { get; set; }
    public int PollingSeconds { get; set; }

    public BankAccountConfig()
    {
        NewBankAccountsQueue = string.Empty;
        DatabaseConnectionString = string.Empty;
    }
}