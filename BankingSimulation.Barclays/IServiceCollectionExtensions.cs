using BankingSimulation.Barclays.Brokers;
using BankingSimulation.Barclays.Services.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSimulation.Barclays;

public static class IServiceCollectionExtensions
{
    public static void AddBankingSimulationBarclaysServices(this IServiceCollection services)
    {
        services.AddScoped<IBarclaysOrchestrationService, BarclaysOrchestrationService>();
        services.AddScoped<IBarclaysAccountProcessingService, BarclaysAccountProcessingService>();
        services.AddScoped<IBarclaysTransactionProcessingService, BarclaysTransactionProcessingService>();
        services.AddScoped<ICSVBroker, CSVBroker>();
    }
}
