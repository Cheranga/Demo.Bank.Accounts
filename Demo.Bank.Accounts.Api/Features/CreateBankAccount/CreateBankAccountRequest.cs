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
        EmployeeId = string.Empty;
    }

    public string CorrelationId { get; set; }
    public string ClientId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public decimal OpeningBalance { get; set; }
    public DateTime CreatedAt { get; set; }
    public string EmployeeId { get; set; }
}