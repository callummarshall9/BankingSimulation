using BankingSimulation.Data.Models;

namespace BankingSimulation.Services.Orchestration
{
    public interface IPeriodAccountSummaryOrchestrationService
    {
        IEnumerable<PeriodAccountSummary> GetCalendarEventAccountSummaries(Guid calendarId);
    }
}