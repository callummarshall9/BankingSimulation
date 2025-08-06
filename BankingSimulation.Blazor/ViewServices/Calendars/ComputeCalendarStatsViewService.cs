using BankingSimulation.Data.Models;
using BankingSimulation.Services.Orchestration;

namespace BankingSimulation.Blazor.ViewServices.Calendars
{
    public class ComputeCalendarStatsViewService(IComputeCalendarStatsOrchestrationService computeCalendarStatsOrchestrationService) : IComputeCalendarStatsViewService
    {
        public IEnumerable<ComputeCalendarStatsResult> ComputeNetCalendarStats(Guid calendarId)
            => computeCalendarStatsOrchestrationService.ComputeNetCalendarStats(calendarId);
    }
}
