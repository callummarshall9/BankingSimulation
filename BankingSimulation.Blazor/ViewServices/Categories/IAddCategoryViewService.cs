using BankingSimulation.Blazor.Models.Categories;

namespace BankingSimulation.Blazor.ViewServices.Categories
{
    public interface IAddCategoryViewService
    {
        Task AddCategoryAsync(AddCategoryViewModel addCategoryViewModel);
    }
}