using BankingSimulation.Data.Models;

namespace BankingSimulation.Services.Orchestration
{
    public interface IComputeCalendarStatsOrchestrationService
    {
        IEnumerable<ComputeCalendarStatsResult> ComputeNetCalendarStats(Guid calendarId);
    }
}