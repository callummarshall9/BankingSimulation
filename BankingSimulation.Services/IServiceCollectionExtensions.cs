using Microsoft.Extensions.DependencyInjection;

namespace BankingSimulation.Services;

public static class IServiceCollectionExtensions
{
    public static void AddBankingSimulationServices(this IServiceCollection services)
    {
        services.AddScoped<IFoundationService, FoundationService>();
    }
}
