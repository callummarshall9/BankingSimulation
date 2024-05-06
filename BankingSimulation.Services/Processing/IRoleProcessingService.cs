using BankingSimulation.Data.Models;

namespace BankingSimulation.Services.Processing
{
    public interface IRoleProcessingService
    {
        Task<Role> AddAsync(Role item);
        Task DeleteAsync(Role item);
        IQueryable<Role> GetAll();
        Task<Role> UpdateAsync(Role item);
    }
}