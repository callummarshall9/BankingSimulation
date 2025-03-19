using BankingSimulation.MBNA.Brokers;
using BankingSimulation.MBNA.Services.Orchestration;
using BankingSimulation.MBNA.Services.Processing;
using BankingSimulation.Services.Orchestration;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSimulation.MBNA;

public static class IServiceCollectionExtensions
{
    public static void AddBankingSimulationMBNAServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<IAccountImportOrchestrationService, MBNAOrchestrationService>("mbna");
        services.AddKeyedScoped<ITransactionImportOrchestrationService, MBNAOrchestrationService>("mbna");
        
        services.AddScoped<IMBNAAccountProcessingService, MBNAAccountProcessingService>();
        services.AddScoped<IMBNATransactionsProcessingService, MBNATransactionsProcessingService>();
        services.AddScoped<ICSVBroker, CSVBroker>();
    }
}