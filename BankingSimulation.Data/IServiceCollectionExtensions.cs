using BankingSimulation.Data.Brokers;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSimulation.Data;

public static class IServiceCollectionExtensions
{
    public static void AddBankingSimulationData(this IServiceCollection services)
    {
        services.AddScoped<IAuthorisationBroker, AuthorisationBroker>();
        services.AddScoped<IStorageBroker, StorageBroker>();
    }
}
