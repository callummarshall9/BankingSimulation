using BankingSimulation.Data.Models;

namespace BankingSimulation.Services.Processing
{
    public interface ICategoryProcessingService
    {
        Task<Category> AddAsync(Category item);
        Task DeleteAsync(Category item);
        IQueryable<Category> GetAll();
        Task<Category> UpdateAsync(Category item);
    }
}