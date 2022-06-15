using Demo.Bank.Accounts.Api.Features.CreateBankAccount;
using Demo.Bank.Accounts.Api.Features.SaveBankAccount;
using Demo.Bank.Accounts.Api.Features.Shared;
using Demo.Bank.Accounts.Api.Infrastructure.DataAccess;
using Demo.Bank.Accounts.Api.Infrastructure.Messaging;
using Demo.Bank.Accounts.Api.Shared;

namespace Demo.Bank.Accounts.Api.Features.ProcessBankAccount;

public class ProcessBankAccountService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageReader _messageReader;
    private readonly ILogger<ProcessBankAccountService> _logger;

    public ProcessBankAccountService(IServiceProvider serviceProvider, 
        IMessageReader messageReader,
        ILogger<ProcessBankAccountService> logger)
    {
        _serviceProvider = serviceProvider;
        _messageReader = messageReader;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var config = scope.ServiceProvider.GetRequiredService<BankAccountConfig>();
                var readMessageOperation = await _messageReader.ReadMessageAsync<CreateBankAccountRequest>(new MessageReadOptions(config.NewBankAccountsQueue, config.VisibilityInSeconds), stoppingToken);

                if (!readMessageOperation.Status || readMessageOperation.Data == null)
                {
                    await Task.Delay(TimeSpan.FromSeconds(config.PollingSeconds), stoppingToken);
                    continue;
                }

                _logger.LogInformation("{CorrelationId} started processing cancellation request", readMessageOperation.Data.CorrelationId);

                var cancellationRequest = readMessageOperation.Data;
                await SaveNewBankAccountAsync(scope, cancellationRequest);

                await Task.Delay(TimeSpan.FromSeconds(config.PollingSeconds), stoppingToken);
            }
        }
    }
    
    private async Task SaveNewBankAccountAsync(IServiceScope scope, CreateBankAccountRequest request)
    {
        var command = new SaveBankAccountCommand
        {
            CorrelationId = request.CorrelationId,
            Address = request.Address,
            Name = request.Name,
            ClientId = request.ClientId,
            OpeningBalance = request.OpeningBalance,
            BankAccountId = request.BankAccountId,
            CreatedAt = DateTime.UtcNow
        };

        var commandHandler = scope.ServiceProvider.GetRequiredService<ICommandHandler<SaveBankAccountCommand>>();

        var operation = await commandHandler.ExecuteAsync(command);
        if (operation.Status)
        {
            _logger.LogInformation("{CorrelationId} bank account successfully saved", request.CorrelationId);
            return;
        }
        
        _logger.LogWarning("{CorrelationId} bank account saving was unsuccessful", request.CorrelationId);
    }
}