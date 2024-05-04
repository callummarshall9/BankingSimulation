using BankingSimulation.Services.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSimulation.Services;

public static class IServiceCollectionExtensions
{
    public static void AddBankingSimulationServices(this IServiceCollection services)
    {
        services.AddScoped<IFoundationService, FoundationService>();
        services.AddScoped<ITransactionProcessingService, TransactionProcessingService>();
        services.AddScoped<ICategoryProcessingService, CategoryProcessingService>();
        services.AddScoped<IAccountProcessingService, AccountProcessingService>();
    }
}
