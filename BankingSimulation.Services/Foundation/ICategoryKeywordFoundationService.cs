using BankingSimulation.Data.Models;

namespace BankingSimulation.Services.Foundation
{
    public interface ICategoryKeywordFoundationService
    {
        Task<CategoryKeyword> AddAsync(CategoryKeyword item);
        Task DeleteAsync(CategoryKeyword item);
        IQueryable<T> GetAll<T>() where T : class;
        Task<CategoryKeyword> UpdateAsync(CategoryKeyword item);
        Task UpdateTransactionsForCategory(Guid categoryId);
    }
}