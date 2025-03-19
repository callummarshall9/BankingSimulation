using BankingSimulation.Services.Orchestration;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSimulation.RBS;

public static class IServiceCollectionExtensions
{
    public static void AddBankingSimulationRBSServices(this IServiceCollection services) 
    {
        services.AddKeyedScoped<IAccountImportOrchestrationService, RBSOrchestrationService>("rbs");
        services.AddKeyedScoped<ITransactionImportOrchestrationService, RBSOrchestrationService>("rbs");
        
        services.AddScoped<IRBSAccountProcessingService, RBSAccountProcessingService>();
        services.AddScoped<IRBSTransactionProcessingService, RBSTransactionProcessingService>();
        services.AddScoped<ICSVBroker, CSVBroker>();
    }
}
