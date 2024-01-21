using BankingSimulation.Data;
using BankingSimulation.Data.Models;

namespace BankingSimulation.Services.Processing
{
    public class TransactionProcessingService
    {
        private readonly IFoundationService foundationService;

        public TransactionProcessingService(IFoundationService foundationService)
        {
            this.foundationService = foundationService;
        }

        public IEnumerable<MonthlyAccountSummary> GetMonthlyAccountSummariesSincePeriod(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            var accounts = foundationService.GetAll<Account>();

            List<MonthlyAccountSummary> accountSummaries = new List<MonthlyAccountSummary>();

            foreach(var account in accounts)
            {
                var transactions = foundationService.GetAll<Transaction>()
                    .Where(t => t.Date >= fromDate && t.Date <= toDate)
                    .GroupBy(t => new { t.Date.Month, t.Date.Year })
                    .Select(g => new MonthlyAccountSummary
                    {
                        Date = DateOnly.FromDateTime(g.First().Date.DateTime),
                        Incomings = g.Where(g => g.Value >= 0.0).Sum(g => g.Value),
                        Outgoings = g.Where(g => g.Value < 0.0).Sum(g => g.Value),
                    })
                    .ToArray();

                accountSummaries.AddRange(transactions);
            }

            return accountSummaries;
        }

    }
}
