using BankingSimulation.Data.Models;
using BankingSimulation.Services.Orchestration;

namespace BankingSimulation.Blazor.ViewServices.Accounts
{
    public class PeriodAccountSummaryViewService(IPeriodAccountSummaryOrchestrationService periodAccountSummaryOrchestrationService) : IPeriodAccountSummaryViewService
    {
        public IEnumerable<PeriodAccountSummary> GetCalendarEventAccountSummaries(Guid calendarId)
            => periodAccountSummaryOrchestrationService.GetCalendarEventAccountSummaries(calendarId);
    }
}