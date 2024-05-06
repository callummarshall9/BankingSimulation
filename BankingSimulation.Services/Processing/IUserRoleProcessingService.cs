using BankingSimulation.Data.Models;

namespace BankingSimulation.Services.Processing
{
    public interface IUserRoleProcessingService
    {
        Task<UserRole> AddAsync(UserRole item);
        Task DeleteAsync(UserRole item);
        IQueryable<UserRole> GetAll();
    }
}