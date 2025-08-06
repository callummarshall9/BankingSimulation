using BankingSimulation.Data.Models;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.Services.Orchestration
{
    public class ComputeCalendarStatsOrchestrationService(ITransactionProcessingService transactionProcessingService,
        ICalendarEventProcessingService calendarEventProcessingService) : IComputeCalendarStatsOrchestrationService
    {
        public IEnumerable<ComputeCalendarStatsResult> ComputeNetCalendarStats(Guid calendarId)
            => calendarEventProcessingService.GetAll()
                .Where(ce => ce.CalendarId == calendarId)
                .Select(ce => new ComputeCalendarStatsResult
                {
                    CalendarEventId = ce.Id,
                    CalendarEventName = ce.Name,
                    CalendarEventStart = ce.Start,
                    FriendlyName = ce.Name,
                    Value = Math.Round(transactionProcessingService
                                .GetAll()
                                .Where(t => t.Date >= ce.Start && t.Date <= ce.End)
                                .Sum(t => t.Value), 2)
                })
                .OrderBy(ccsr => ccsr.CalendarEventStart)
                .ToList();
    }
}
