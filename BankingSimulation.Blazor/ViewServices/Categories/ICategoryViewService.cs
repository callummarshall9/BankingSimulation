using BankingSimulation.Blazor.Models.Categories;

namespace BankingSimulation.Blazor.ViewServices.Categories
{
    public interface ICategoryViewService
    {
        IEnumerable<CategoryViewModel> GetCategories();
        Task DeleteCategoryAsync(Guid categoryId);
    }
}