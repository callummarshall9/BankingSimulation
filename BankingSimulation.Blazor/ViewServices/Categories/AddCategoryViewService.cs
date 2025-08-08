using BankingSimulation.Blazor.Models.Categories;
using BankingSimulation.Services.Orchestration;

namespace BankingSimulation.Blazor.ViewServices.Categories
{
    public class AddCategoryViewService(ICategoryRoleOrchestrationService categoryRoleOrchestrationService) : IAddCategoryViewService
    {
        public Task AddCategoryAsync(AddCategoryViewModel addCategoryViewModel)
            => categoryRoleOrchestrationService.AddCategoryAsync(
                addCategoryViewModel.Name,
                addCategoryViewModel.Description
            );
    }
}
