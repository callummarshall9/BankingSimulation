using Microsoft.Extensions.DependencyInjection;

namespace BankingSimulation.Data;

public static class IServiceCollectionExtensions
{
    public static void AddBankingSimulationData(this IServiceCollection services)
    {
        services.AddDbContext<BankSimulationContext>();
        services.AddScoped<IStorageBroker, StorageBroker>();
    }
}
