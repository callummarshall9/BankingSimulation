using System.Data;
using BankingSimulation.BlazorServer.ViewModels.CategoryKeywords;
using BankingSimulation.Data.Models;
using BankingSimulation.Services;

namespace BankingSimulation.BlazorServer.ViewServices.CategoryKeywords;

public class CategoryKeywordsViewService(IFoundationService foundationService) : ICategoryKeywordsViewService
{
    public CategoryKeywordAggregateViewModel Index(Guid categoryId)
    {
        return new CategoryKeywordAggregateViewModel()
        {
            CategoryId = categoryId,
            Keywords = foundationService.GetAll<CategoryKeyword>()
                .Where(ck => ck.CategoryId == categoryId)
                .Select(ck => new CategoryKeywordViewModel()
                {
                    Id = ck.Id,
                    Keyword = ck.Keyword
                }).ToList()
        };
    }

    public async Task<CategoryKeywordAggregateViewModel> DeleteAsync(CategoryKeywordViewModel keyword,
        CategoryKeywordAggregateViewModel model)
    {
        try
        {
            var existing = foundationService
                .GetAll<CategoryKeyword>()
                .FirstOrDefault(c => c.Id == keyword.Id);

            if (existing == null)
                throw new DataException("Category keyword not found");

            await foundationService.DeleteAsync(existing);

            return Index(model.CategoryId);

        }
        catch (Exception ex)
        {
            keyword.Deleting = false;
            keyword.Exception = ex;

            return model;
        }
    }
}