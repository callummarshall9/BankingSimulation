using BankingSimulation.Data;
using BankingSimulation.Data.Models;

namespace BankingSimulation.Services.Processing
{
    public interface ITransactionProcessingService
    {
        Task<Transaction> AddAsync(Transaction item);
        Task DeleteAsync(Transaction item);
        IQueryable<Transaction> GetAll();
        IEnumerable<PeriodAccountSummary> GetCalendarEventAccountSummaries(Guid calendarId);
        Task<Transaction> UpdateAsync(Transaction item);
    }
}