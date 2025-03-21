using BankingSimulation.BlazorServer.ViewModels.CategoryKeywords;
using BankingSimulation.Data.Models;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.BlazorServer.ViewServices.CategoryKeywords;

public class AddCategoryKeywordsViewService(ICategoryKeywordsProcessingService processingService) : IAddCategoryKeywordsViewService
{
    public async Task<AddNewCategoryKeywordViewModel> AddAsync(AddNewCategoryKeywordViewModel model)
    {
        try
        {
            if(string.IsNullOrEmpty(model.Keyword))
                throw new ArgumentException("Name cannot be null or empty");
        
            await processingService.AddAsync(new CategoryKeyword()
            {
                Keyword =  model.Keyword,
                CategoryId = model.CategoryId
            });

            return new AddNewCategoryKeywordViewModel()
            {
                Keyword = model.Keyword,
                CategoryId = model.CategoryId,
                Loading = false,
                Exception = null,
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new AddNewCategoryKeywordViewModel()
            {
                Keyword = model.Keyword,
                CategoryId = model.CategoryId,
                Loading = false,
                Exception = ex,
                Success = false
            };
        }
    }
}