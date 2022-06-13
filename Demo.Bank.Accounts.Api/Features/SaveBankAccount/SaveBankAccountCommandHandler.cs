using Demo.Bank.Accounts.Api.Features.Shared;
using Demo.Bank.Accounts.Api.Infrastructure.DataAccess;
using FluentValidation;

namespace Demo.Bank.Accounts.Api.Features.SaveBankAccount;

public class SaveBankAccountCommandHandler : CommandHandlerBase<SaveBankAccountCommand>
{
    public SaveBankAccountCommandHandler(IValidator<SaveBankAccountCommand> validator, BankAccountConfig config, ILogger<SaveBankAccountCommandHandler> logger) : base(validator, config, logger)
    {
    }

    protected override string CommandText => @"insert into tblBankAccounts (BankAccountId, CorrelationId, ClientId, Name, Address, OpeningBalance, CreatedAt) " +
                                             "output inserted.Id, inserted.BankAccountId, inserted.CorrelationId, inserted.ClientId, inserted.Name, inserted.Address, inserted.OpeningBalance, inserted.CreatedAt " +
                                             "values (@BankAccountId, @CorrelationId, @ClientId, @Name, @Address, @OpeningBalance, @CreatedAt)";
}