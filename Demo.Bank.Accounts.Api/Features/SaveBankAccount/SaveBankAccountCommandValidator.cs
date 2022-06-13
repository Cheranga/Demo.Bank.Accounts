using Demo.Bank.Accounts.Api.Shared;
using FluentValidation;

namespace Demo.Bank.Accounts.Api.Features.SaveBankAccount;

public class SaveBankAccountCommandValidator : ModelValidatorBase<SaveBankAccountCommand>
{
    public SaveBankAccountCommandValidator()
    {
        RuleFor(x => x.CorrelationId).NotNull().NotEmpty().WithMessage("correlationId is required");
        RuleFor(x => x.ClientId).NotNull().NotEmpty().WithMessage("clientId is required");
    }
}