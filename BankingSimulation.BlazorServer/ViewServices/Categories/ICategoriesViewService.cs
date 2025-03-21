using BankingSimulation.BlazorServer.ViewModels.Categories;

namespace BankingSimulation.BlazorServer.ViewServices.Categories;

public interface ICategoriesViewService
{
    CategoriesViewModel Index();
    Task<CategoriesViewModel> DeleteAsync(CategoryViewModel category, CategoriesViewModel model);
}