using Microsoft.Extensions.DependencyInjection;

namespace BankingSimulation.RBS;

public static class IServiceCollectionExtensions
{
    public static void AddBankingSimulationRBSServices(this IServiceCollection services) 
    {
        services.AddScoped<IRBSOrchestrationService, RBSOrchestrationService>();
        services.AddScoped<IRBSAccountProcessingService, RBSAccountProcessingService>();
        services.AddScoped<IRBSTransactionProcessingService, RBSTransactionProcessingService>();
        services.AddScoped<ICSVBroker, CSVBroker>();
    }
}
