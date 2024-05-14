using BankingSimulation.BarclaysCard.Brokers;
using BankingSimulation.BarclaysCard.Services.Orchestration;
using BankingSimulation.BarclaysCard.Services.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSimulation.BarclaysCard;

public static class IServiceCollectionExtensions
{
    public static void AddBankingSimulationBarclaysCardServices(this IServiceCollection services)
    {
        services.AddScoped<IBarclaysCardOrchestrationService, BarclaysCardOrchestrationService>();
        services.AddScoped<IBarclaysCardAccountProcessingService, BarclaysCardAccountProcessingService>();
        services.AddScoped<IBarclaysCardTransactionProcessingService, BarclaysCardTransactionProcessingService>();
        services.AddScoped<ICSVBroker, CSVBroker>();
    }
}