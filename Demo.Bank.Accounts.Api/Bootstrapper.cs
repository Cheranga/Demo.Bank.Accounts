using Azure.Identity;
using Demo.Bank.Accounts.Api.Features.CreateBankAccount;
using Demo.Bank.Accounts.Api.Features.SaveBankAccount;
using Demo.Bank.Accounts.Api.Features.Shared;
using Demo.Bank.Accounts.Api.Features.TransferMoney;
using Demo.Bank.Accounts.Api.Infrastructure.DataAccess;
using Demo.Bank.Accounts.Api.Infrastructure.Messaging;
using Demo.Bank.Accounts.Api.Shared;
using FluentValidation;
using Microsoft.Extensions.Azure;

namespace Demo.Bank.Accounts.Api;

public static class Bootstrapper
{
    public static void RegisterDependencies(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        RegisterConfigs(builder);
        RegisterAzureClients(builder);
        RegisterValidators(services);
        RegisterApplicationServices(services);
        RegisterResponseCreators(services);
        RegisterMessaging(services);
        RegisterDataAccess(services);
    }

    private static void RegisterDataAccess(IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<SaveBankAccountCommand>, SaveBankAccountCommandHandler>();
    }

    private static void RegisterConfigs(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(_ =>
        {
            var config = builder.Configuration.GetSection(nameof(BankAccountConfig)).Get<BankAccountConfig>();
            return config;
        });
    }

    private static void RegisterMessaging(IServiceCollection services)
    {
        services.AddScoped<IMessagePublisher, MessagePublisher>();
    }

    private static void RegisterAzureClients(WebApplicationBuilder builder)
    {
        var storageAccount = builder.Configuration["StorageAccount"];
        
        builder.Services.AddAzureClients(clientBuilder =>
        {
            if (builder.Environment.IsDevelopment())
            {
                clientBuilder.AddQueueServiceClient(storageAccount);
                return;
            }

            var credentials = new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                ExcludeManagedIdentityCredential = false,

                ExcludeEnvironmentCredential = true,
                ExcludeAzureCliCredential = true,
                ExcludeInteractiveBrowserCredential = true,
                ExcludeVisualStudioCredential = true,
                ExcludeAzurePowerShellCredential = true,
                ExcludeSharedTokenCacheCredential = true,
                ExcludeVisualStudioCodeCredential = true
            });

            clientBuilder.AddQueueServiceClient(new Uri($"https://{storageAccount}.queue.core.windows.net")).WithCredential(credentials);
            clientBuilder.AddTableServiceClient(new Uri($"https://{storageAccount}.table.core.windows.net")).WithCredential(credentials);
        });
    }

    private static void RegisterResponseCreators(IServiceCollection services)
    {
        services.AddScoped<IResponseCreator<CreateBankAccountRequest>, CreateBankAccountResponseCreator>();
        services.AddScoped<IResponseCreator<TransferMoneyRequest>, TransferMoneyResponseCreator>();
    }

    private static void RegisterValidators(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ModelValidatorBase<>).Assembly);
    }

    private static void RegisterApplicationServices(IServiceCollection services)
    {
        services.AddScoped<ICreateBankAccountService, CreateBankAccountService>();
        services.AddScoped<ITransferMoneyService, TransferMoneyService>();
    }
}