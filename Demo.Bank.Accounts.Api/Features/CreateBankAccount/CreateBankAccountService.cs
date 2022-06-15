using Demo.Bank.Accounts.Api.Features.SaveBankAccount;
using Demo.Bank.Accounts.Api.Features.Shared;
using Demo.Bank.Accounts.Api.Infrastructure.DataAccess;
using Demo.Bank.Accounts.Api.Infrastructure.Messaging;
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
    private readonly BankAccountConfig _config;
    private readonly IMessagePublisher _messagePublisher;

    public CreateBankAccountService(IValidator<CreateBankAccountRequest> validator,
        BankAccountConfig config,
        IMessagePublisher messagePublisher,
        ILogger<CreateBankAccountService> logger)
    {
        _validator = validator;
        _config = config;
        _messagePublisher = messagePublisher;
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

        var command = new SaveBankAccountCommand
        {
            Address = request.Address,
            Name = request.Name,
            ClientId = request.ClientId,
            CorrelationId = request.CorrelationId,
            CreatedAt = DateTime.UtcNow,
            OpeningBalance = request.OpeningBalance,
            BankAccountId = request.BankAccountId
        };

        _logger.LogInformation("{CorrelationId} create bank account request received", request.CorrelationId);
        
        var operation = await _messagePublisher.PublishAsync(_config.NewBankAccountsQueue, request);
        if (!operation.Status)
        {
            _logger.LogError("{CorrelationId} cannot create account", request.CorrelationId);
            return operation;
        }

        _logger.LogInformation("{CorrelationId} bank account creation request submitted successfully", request.CorrelationId);
        return Result.Success();
    }
}