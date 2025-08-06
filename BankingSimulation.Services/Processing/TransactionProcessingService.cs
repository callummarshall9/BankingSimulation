using BankingSimulation.Data;
using BankingSimulation.Data.Models;
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

    }
}
