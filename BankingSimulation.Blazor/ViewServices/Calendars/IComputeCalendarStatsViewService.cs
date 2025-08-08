using BankingSimulation.Data.Models;

namespace BankingSimulation.Blazor.ViewServices.Calendars
{
    public interface IComputeCalendarStatsViewService
    {
        IEnumerable<ComputeCalendarStatsResult> ComputeNetCalendarStats(Guid calendarId);
    }
}