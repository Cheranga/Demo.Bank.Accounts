using Demo.Bank.Accounts.Api.Shared;
using FluentValidation;

namespace Demo.Bank.Accounts.Api.Features.TransferMoney;

public class TransferMoneyRequestValidator : ModelValidatorBase<TransferMoneyRequest>
{
    public TransferMoneyRequestValidator()
    {
        RuleFor(x => x.CorrelationId).NotNull().NotEmpty().WithMessage("correlationId is required");
        RuleFor(x => x.CustomerId).NotNull().NotEmpty().WithMessage("customerId is required");
        RuleFor(x => x.FromAccount).NotNull().NotEmpty().WithMessage("fromAccount is required");
        RuleFor(x => x.ToAccount).NotNull().NotEmpty().WithMessage("toAccount is required");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("the transfer amount must be greater than zero");
    }
}