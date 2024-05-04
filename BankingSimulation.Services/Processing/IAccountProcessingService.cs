using BankingSimulation.Data;

namespace BankingSimulation.Services.Processing
{
    public interface IAccountProcessingService
    {
        Task<Account> AddAsync(Account item);
        Task DeleteAsync(Account item);
        IQueryable<Account> GetAll();
        Task<Account> UpdateAsync(Account item);
    }
}