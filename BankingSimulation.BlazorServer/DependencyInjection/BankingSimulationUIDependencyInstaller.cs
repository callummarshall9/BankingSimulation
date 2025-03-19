using BankingSimulation.UI.ViewModels.ImportData;
using BankingSimulation.UI.ViewServices;

namespace BankingSimulation.UI;

public class BankingSimulationUIDependencyInstaller
{
    public static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IAccountSummaryChartViewService, AccountSummaryChartViewService>();
        services.AddScoped<IBankAccountsViewService, BankAccountsViewService>();
        services.AddScoped<IImportDataViewService, ImportDataViewService>();
        services.AddScoped<ICalendarViewService, CalendarViewService>();
    }
}