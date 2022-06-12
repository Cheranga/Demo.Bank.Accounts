using Demo.Bank.Accounts.Api.Features.CreateBankAccount;
using Demo.Bank.Accounts.Api.Features.Shared;
using Demo.Bank.Accounts.Api.Features.TransferMoney;
using Demo.Bank.Accounts.Api.Shared;
using FluentValidation;

namespace Demo.Bank.Accounts.Api;

public static class Bootstrapper
{
    public static void RegisterDependencies(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        
        RegisterValidators(services);
        RegisterApplicationServices(services);
        RegisterResponseCreators(services);
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