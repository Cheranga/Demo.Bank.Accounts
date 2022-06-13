using Demo.Bank.Accounts.Api.Shared;
using FluentValidation;

namespace Demo.Bank.Accounts.Api.Features.CreateBankAccount;

public class CreateBankAccountRequestValidator : ModelValidatorBase<CreateBankAccountRequest>
{
    public CreateBankAccountRequestValidator()
    {
        RuleFor(x => x.CorrelationId).NotNull().NotEmpty().WithMessage("correlationId is required");
        // RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("name is required");
        RuleFor(x => x.Address).NotNull().NotEmpty().WithMessage("address is required");
        RuleFor(x => x.OpeningBalance).GreaterThan(0).WithMessage("there must be an opening balance");
    }
}