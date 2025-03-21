using BankingSimulation.BlazorServer.ViewModels.CategoryKeywords;

namespace BankingSimulation.BlazorServer.ViewServices.CategoryKeywords;

public interface IAddCategoryKeywordsViewService
{
    Task<AddNewCategoryKeywordViewModel> AddAsync(AddNewCategoryKeywordViewModel model);
}