using BankingSimulation.Blazor.Models.Categories;

namespace BankingSimulation.Blazor.ViewServices.Categorys
{
    public interface IEditCategoryViewService
    {
        EditCategoryViewModel GetCategory(Guid categoryId);
        Task<EditCategoryViewModel> UpdateCategoryAsync(EditCategoryViewModel editCategoryViewModel);
    }
}