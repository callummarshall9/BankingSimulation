using BankingSimulation.Data.Models;

namespace BankingSimulation.Services.Orchestration
{
    public interface ICategoryRoleOrchestrationService
    {
        Task<Category> AddCategoryAsync(string? name, string? description);
    }
}