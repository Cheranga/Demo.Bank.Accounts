using Demo.Bank.Accounts.Api.Shared;
using FluentValidation;

namespace Demo.Bank.Accounts.Api.Features.CreateBankAccount;

public interface ICreateBankAccountService
{
    Task<Result> CreateAccountAsync(CreateBankAccountRequest request);
}

public class CreateBankAccountService : ICreateBankAccountService
{
    private readonly ILogger<CreateBankAccountService> _logger;
    private readonly IValidator<CreateBankAccountRequest> _validator;

    public CreateBankAccountService(IValidator<CreateBankAccountRequest> validator,
        ILogger<CreateBankAccountService> logger)
    {
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result> CreateAccountAsync(CreateBankAccountRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Invalid request received {@InvalidRequest}", request);
            return Result.Failure(ErrorCodes.InvalidRequest, validationResult);
        }

        // TODO: implement the rest of the functionality.
        _logger.LogInformation("{CorrelationId} creating bank account", request.CorrelationId);
        return Result.Success();
    }
}