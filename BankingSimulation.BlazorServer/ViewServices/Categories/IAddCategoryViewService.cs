using BankingSimulation.BlazorServer.ViewModels.Categories;

namespace BankingSimulation.BlazorServer.ViewServices.Categories;

public interface IAddCategoryViewService
{
    Task<AddNewCategoryViewModel> AddAsync(AddNewCategoryViewModel model);
}