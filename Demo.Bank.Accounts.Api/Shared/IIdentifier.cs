namespace Demo.Bank.Accounts.Api.Shared;

public interface IIdentifier
{
    string CorrelationId { get; set; }
}