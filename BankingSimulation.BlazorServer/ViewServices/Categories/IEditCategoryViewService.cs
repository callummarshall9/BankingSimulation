using BankingSimulation.BlazorServer.ViewModels.Categories;

namespace BankingSimulation.BlazorServer.ViewServices.Categories;

public interface IEditCategoryViewService
{
    EditCategoryViewModel Index(Guid categoryId);
    Task<EditCategoryViewModel> UpdateAsync(EditCategoryViewModel model);
}