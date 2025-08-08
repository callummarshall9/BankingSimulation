using BankingSimulation.Data.Models;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.Services.Orchestration
{
    public class PeriodAccountSummaryOrchestrationService(IAccountProcessingService accountProcessingService,
        ICalendarEventProcessingService calendarEventProcessingService,
        ITransactionProcessingService transactionProcessingService) : IPeriodAccountSummaryOrchestrationService
    {
        public IEnumerable<PeriodAccountSummary> GetCalendarEventAccountSummaries(Guid calendarId)
        {
            var accounts = accountProcessingService.GetAll()
                .ToList();

            var calendarEvents = calendarEventProcessingService.GetAll()
                .Where(ce => ce.CalendarId == calendarId)
                .ToList();

            var results = new List<PeriodAccountSummary>();

            foreach (var a in accounts)
            {
                foreach (var ce in calendarEvents)
                {
                    results.Add(new PeriodAccountSummary
                    {
                        AccountId = a.Id,
                        AccountName = a.Name,
                        AccountNumber = a.Number,
                        ComputedAccountInfo = a.Name + " (" + a.Number + ")",
                        CalendarEventName = ce.Name,
                        CalendarEventStart = ce.Start,
                        CalendarEventEnd = ce.End,
                        Incomings = Math.Round(transactionProcessingService.GetAll()
                            .Where(t =>
                                t.Date >= ce.Start
                                && t.Date <= ce.End
                                && t.Value >= 0.0
                                && t.AccountId == a.Id
                            )
                            .Sum(l => l.Value), 2),
                        Outgoings = Math.Round(transactionProcessingService.GetAll()
                            .Where(t =>
                                t.Date >= ce.Start
                                && t.Date <= ce.End
                                && t.Value <= 0.0
                                && t.AccountId == a.Id)
                            .Sum(l => l.Value), 2)
                    });
                }
            }

            return results
                .OrderBy(pas => pas.CalendarEventStart)
                .ThenBy(pas => pas.AccountName)
                .ToList();
        }
    }
}
