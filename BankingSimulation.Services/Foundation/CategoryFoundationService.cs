using BankingSimulation.Data;
using BankingSimulation.Data.Brokers;
using BankingSimulation.Data.Models;
using System.Security;

namespace BankingSimulation.Services.Foundation
{
    public class CategoryKeywordFoundationService(IStorageBroker storageBroker,
        IAuthorisationBroker authorisationBroker) : ICategoryKeywordFoundationService
    {
        public async Task<CategoryKeyword> AddAsync(CategoryKeyword item)
        {
            if (authorisationBroker.GetUserId() == "Guest")
                throw new SecurityException("Access Denied!");

            return await storageBroker.AddAsync(item);
        }

        public IQueryable<T> GetAll<T>() where T : class
            => storageBroker.GetAll<T>();

        public async Task<CategoryKeyword> UpdateAsync(CategoryKeyword item)
        {
            if (authorisationBroker.GetUserId() == "Guest")
                throw new SecurityException("Access Denied!");

            return await storageBroker.UpdateAsync(item);
        }

        public async Task DeleteAsync(CategoryKeyword item)
        {
            if (authorisationBroker.GetUserId() == "Guest")
                throw new SecurityException("Access Denied!");

            await storageBroker.DeleteAsync(item);
        }

        public Task UpdateTransactionsForCategory(Guid categoryId)
            => storageBroker.UpdateTransactionsForCategory(categoryId);
    }
}
