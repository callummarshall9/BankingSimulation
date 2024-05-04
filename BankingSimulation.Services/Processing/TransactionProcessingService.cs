using BankingSimulation.Data;
using BankingSimulation.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace BankingSimulation.Services.Processing
{
    public class TransactionProcessingService(IFoundationService foundationService) : ITransactionProcessingService
    {
        public Task<Transaction> AddAsync(Transaction item)
        {
            item.Account = null;
            item.TransactionType = null;

            bool accountExists = foundationService.GetAll<Account>()
                .Any(a => a.Id == item.AccountId);

            if (!accountExists)
                throw new SecurityException("Access Denied!");

            return foundationService.AddAsync(item);
        }

        public Task<Transaction> UpdateAsync(Transaction item)
        {
            item.Account = null;
            item.TransactionType = null;

            bool accountExists = foundationService.GetAll<Account>()
                .Any(a => a.Id == item.AccountId);

            if (!accountExists)
                throw new SecurityException("Access Denied!");

            return foundationService.UpdateAsync(item);
        }

        public Task DeleteAsync(Transaction item)
        {
            item.Account = null;
            item.TransactionType = null;

            bool accountExists = foundationService.GetAll<Account>()
                .Any(a => a.Id == item.AccountId);

            if (!accountExists)
                throw new SecurityException("Access Denied!");

            return foundationService.DeleteAsync(item);
        }

        public IQueryable<Transaction> GetAll()
            => foundationService.GetAll<Transaction>();

        public IEnumerable<MonthlyAccountSummary> GetMonthlyAccountSummariesSincePeriod(DateOnly fromDate, DateOnly toDate)
        {
            var accounts = foundationService.GetAll<Account>();

            List<MonthlyAccountSummary> accountSummaries = new List<MonthlyAccountSummary>();

            foreach (var account in accounts)
            {
                var transactions = foundationService.GetAll<Transaction>()
                    .Where(t => t.Date >= fromDate && t.Date <= toDate)
                    .GroupBy(t => new { t.Date.Month, t.Date.Year, t.AccountId })
                    .Select(g => new MonthlyAccountSummary
                    {
                        Date = g.First().Date,
                        AccountId = g.First().AccountId,
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
