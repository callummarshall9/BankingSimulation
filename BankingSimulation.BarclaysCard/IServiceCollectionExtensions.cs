using BankingSimulation.BarclaysCard.Brokers;
using BankingSimulation.BarclaysCard.Services.Orchestration;
using BankingSimulation.BarclaysCard.Services.Processing;
using BankingSimulation.Services.Orchestration;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSimulation.BarclaysCard;

public static class IServiceCollectionExtensions
{
    public static void AddBankingSimulationBarclaysCardServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<IAccountImportOrchestrationService, BarclaysCardOrchestrationService>("barclaysCard");
        services.AddKeyedScoped<ITransactionImportOrchestrationService, BarclaysCardOrchestrationService>("barclaysCard");
        
        services.AddScoped<IBarclaysCardAccountProcessingService, BarclaysCardAccountProcessingService>();
        services.AddScoped<IBarclaysCardTransactionProcessingService, BarclaysCardTransactionProcessingService>();
        services.AddScoped<ICSVBroker, CSVBroker>();
    }
}