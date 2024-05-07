using BankingSimulation.Services.Foundation;
using BankingSimulation.Services.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSimulation.Services;

public static class IServiceCollectionExtensions
{
    public static void AddBankingSimulationServices(this IServiceCollection services)
    {
        services.AddScoped<IFoundationService, FoundationService>();
        services.AddScoped<ICategoryKeywordFoundationService, CategoryKeywordFoundationService>();

        services.AddScoped<IAccountProcessingService, AccountProcessingService>();
        services.AddScoped<ICalendarProcessingService, CalendarProcessingService>();
        services.AddScoped<ICalendarEventProcessingService, CalendarEventProcessingService>();
        services.AddScoped<ICategoryProcessingService, CategoryProcessingService>();
        services.AddScoped<ICategoryKeywordsProcessingService, CategoryKeywordsProcessingService>();
        services.AddScoped<IRoleProcessingService, RoleProcessingService>();
        services.AddScoped<IUserRoleProcessingService, UserRoleProcessingService>();
        services.AddScoped<ITransactionProcessingService, TransactionProcessingService>();
    }
}
