using BankingSimulation.Blazor.Models.Accounts;

namespace BankingSimulation.Blazor.ViewServices.Accounts
{
    public interface ICalendarViewService
    {
        IEnumerable<CalendarViewModel> GetCalendarViewModels();
    }
}