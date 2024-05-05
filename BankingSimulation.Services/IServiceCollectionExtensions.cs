using BankingSimulation.Services.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSimulation.Services;

public static class IServiceCollectionExtensions
{
    public static void AddBankingSimulationServices(this IServiceCollection services)
    {
        services.AddScoped<IFoundationService, FoundationService>();

        services.AddScoped<IAccountProcessingService, AccountProcessingService>();
        services.AddScoped<ICategoryProcessingService, CategoryProcessingService>();
        services.AddScoped<IRoleProcessingService, RoleProcessingService>();
        services.AddScoped<ITransactionProcessingService, TransactionProcessingService>();
    }
}
