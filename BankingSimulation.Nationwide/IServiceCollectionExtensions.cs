using BankingSimulation.Nationwide.Brokers;
using BankingSimulation.Nationwide.Services.Orchestration;
using BankingSimulation.Nationwide.Services.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSimulation.Nationwide
{
    public static class IServiceCollectionExtensions
    {
        public static void AddBankingSimulationNationwideServices(this IServiceCollection services)
        {
            services.AddScoped<INationwideOrchestrationService, NationwideOrchestrationService>();
            services.AddScoped<INationwideAccountProcessingService, NationwideAccountProcessingService>();
            services.AddScoped<INationwideTransactionProcessingService, NationwideTransactionProcessingService>();
            services.AddScoped<ICSVBroker, CSVBroker>();
        }
    }
}