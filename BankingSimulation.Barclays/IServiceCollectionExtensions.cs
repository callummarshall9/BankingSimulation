using BankingSimulation.Barclays.Brokers;
using BankingSimulation.Barclays.Services.Processing;
using BankingSimulation.Services.Orchestration;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSimulation.Barclays;

public static class IServiceCollectionExtensions
{
    public static void AddBankingSimulationBarclaysServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<IAccountImportOrchestrationService, BarclaysOrchestrationService>("barclays");
        services.AddKeyedScoped<ITransactionImportOrchestrationService, BarclaysOrchestrationService>("barclays");
        
        services.AddScoped<IBarclaysAccountProcessingService, BarclaysAccountProcessingService>();
        services.AddScoped<IBarclaysTransactionProcessingService, BarclaysTransactionProcessingService>();
        services.AddScoped<ICSVBroker, CSVBroker>();
    }
}
