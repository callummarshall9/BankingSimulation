using BankingSimulation.Data.Models;

namespace BankingSimulation.Blazor.ViewServices.Accounts
{
    public interface IPeriodAccountSummaryViewService
    {
        IEnumerable<PeriodAccountSummary> GetCalendarEventAccountSummaries(Guid calendarId);
    }
}