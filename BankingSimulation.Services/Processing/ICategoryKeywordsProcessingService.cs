using BankingSimulation.Data.Models;

namespace BankingSimulation.Services.Processing
{
    public interface ICategoryKeywordsProcessingService
    {
        Task<CategoryKeyword> AddAsync(CategoryKeyword item);
        Task DeleteAsync(CategoryKeyword item);
        IQueryable<CategoryKeyword> GetAll();
        Task<CategoryKeyword> UpdateAsync(CategoryKeyword item);
    }
}