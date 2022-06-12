using System.Text.Json.Serialization;

namespace Demo.Bank.Accounts.Api.Features.CreateBankAccount;

public class CreateBankAccountDto
{
    public CreateBankAccountDto()
    {
        CorrelationId = string.Empty;
        Name = string.Empty;
        Address = string.Empty;
        OpeningBalance = 0;
    }

    [JsonIgnore] public string CorrelationId { get; set; }

    public string Name { get; set; }
    public string Address { get; set; }
    public decimal OpeningBalance { get; set; }
}