using BankingSimulation.Data;
using BankingSimulation.Data.Models;

namespace BankingSimulation.Services.Processing
{
    public interface ITransactionProcessingService
    {
        Task<Transaction> AddAsync(Transaction item);
        Task DeleteAsync(Transaction item);
        IQueryable<Transaction> GetAll();
        IEnumerable<MonthlyAccountSummary> GetMonthlyAccountSummariesSincePeriod(DateOnly fromDate, DateOnly toDate);
        Task<Transaction> UpdateAsync(Transaction item);
    }
}