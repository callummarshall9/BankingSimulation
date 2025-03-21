using BankingSimulation.BlazorServer.ViewServices.CalendarEvents;
using BankingSimulation.BlazorServer.ViewServices.Categories;
using BankingSimulation.BlazorServer.ViewServices.CategoryKeywords;
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
        services.AddScoped<IAddCalendarViewService, AddCalendarViewService>();

        services.AddScoped<IAddCalendarEventViewService, AddCalendarEventViewService>();
        services.AddScoped<ICalendarEventsViewService, CalendarEventsViewService>();
        
        services.AddScoped<IAddCalendarViewService, AddCalendarViewService>();
        services.AddScoped<IEditCalendarViewService, EditCalendarViewService>();
        services.AddScoped<ICalendarViewService, CalendarViewService>();

        services.AddScoped<ICategoriesViewService, CategoriesViewService>();
        services.AddScoped<IAddCategoryViewService, AddCategoryViewService>();
        services.AddScoped<IEditCategoryViewService, EditCategoryViewService>();

        services.AddScoped<IAddCategoryKeywordsViewService, AddCategoryKeywordsViewService>();
        services.AddScoped<ICategoryKeywordsViewService, CategoryKeywordsViewService>();
    }
}