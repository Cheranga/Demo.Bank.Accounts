using Demo.Bank.Accounts.Api.Shared;
using FluentValidation;

namespace Demo.Bank.Accounts.Api.Features.TransferMoney;

public interface ITransferMoneyService
{
    Task<Result> TransferMoneyAsync(TransferMoneyRequest request);
}

public class TransferMoneyService : ITransferMoneyService
{
    private readonly ILogger<TransferMoneyService> _logger;
    private readonly IValidator<TransferMoneyRequest> _validator;

    public TransferMoneyService(IValidator<TransferMoneyRequest> validator,
        ILogger<TransferMoneyService> logger)
    {
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<Result> TransferMoneyAsync(TransferMoneyRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Invalid request received {@InvalidRequest}", request);
            return Result.Failure(ErrorCodes.InvalidRequest, validationResult);
        }

        // TODO: implement the rest of the functionality.
        return Result.Success();
    }
}