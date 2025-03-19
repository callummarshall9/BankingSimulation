using BankingSimulation.Data.Models;

namespace BankingSimulation.UI.ViewServices;

public interface IAccountSummaryChartViewService
{
    IEnumerable<PeriodAccountSummary> GetPeriodAccountSummaries(Guid calendarId);
}