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

        public IEnumerable<PeriodAccountSummary> GetCalendarEventAccountSummaries(Guid calendarId)
        {
            var accounts = foundationService.GetAll<Account>()
                .ToList();
            
            var calendarEvents = foundationService.GetAll<CalendarEvent>()
                .Where(ce => ce.CalendarId == calendarId)
                .ToList();
            
            var results = new List<PeriodAccountSummary>();

            foreach (var a in accounts)
            {
                foreach(var ce in calendarEvents)
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
                        Incomings = Math.Round(foundationService.GetAll<Transaction>()
                            .Where(t => 
                                t.Date >= ce.Start 
                                && t.Date <= ce.End 
                                && t.Value >= 0.0 
                                && t.AccountId == a.Id
                            )
                            .Sum(l => l.Value), 2),
                        Outgoings = Math.Round(foundationService.GetAll<Transaction>()
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
