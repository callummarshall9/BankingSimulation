using BankingSimulation.BlazorServer.ViewModels.CategoryKeywords;

namespace BankingSimulation.BlazorServer.ViewServices.CategoryKeywords;

public interface ICategoryKeywordsViewService
{
    CategoryKeywordAggregateViewModel Index(Guid categoryId);

    Task<CategoryKeywordAggregateViewModel> DeleteAsync(CategoryKeywordViewModel keyword,
        CategoryKeywordAggregateViewModel model);
}