using System.Data.SqlClient;
using System.Transactions;
using Dapper;
using Demo.Bank.Accounts.Api.Features.Shared;
using Demo.Bank.Accounts.Api.Shared;
using FluentValidation;

namespace Demo.Bank.Accounts.Api.Infrastructure.DataAccess;

public interface ICommand : IIdentifier
{
}

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<Result> ExecuteAsync(TCommand command);
}

public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly IValidator<TCommand> _validator;
    private readonly BankAccountConfig _config;
    private readonly ILogger<CommandHandlerBase<TCommand>> _logger;

    protected CommandHandlerBase(IValidator<TCommand> validator, BankAccountConfig config, ILogger<CommandHandlerBase<TCommand>> logger)
    {
        _validator = validator;
        _config = config;
        _logger = logger;
    }

    protected abstract string CommandText { get; }

    public async Task<Result> ExecuteAsync(TCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            _logger.LogError("{CorrelationId} invalid command", command.CorrelationId);
            return Result.Failure(ErrorCodes.InvalidCommand, validationResult);
        }

        try
        {
            using (var connection = new SqlConnection(_config.DatabaseConnectionString))
            {
                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        await connection.OpenAsync();
                        
                        var queryResults = await connection.QueryAsync(CommandText, command);
                        var data = queryResults?.FirstOrDefault();
                        if (data == null)
                        {
                            _logger.LogError("{CorrelationId} command execution unsuccessful", command.CorrelationId);
                            return Result.Failure(ErrorCodes.CommandError, ErrorMessages.CommandError);
                        }
                    
                        _logger.LogInformation("{CorrelationId} command execution successful", command.CorrelationId);
                        transactionScope.Complete();
                        return Result.Success();
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, "error occurred when executing the transaction");
                        return Result.Failure(ErrorCodes.CommandError, ErrorMessages.CommandError);
                    }
                }
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "{CorrelationId} error occurred when executing command", command.CorrelationId);
        }

        return Result.Failure(ErrorCodes.CommandError, ErrorMessages.CommandError);
    }
}