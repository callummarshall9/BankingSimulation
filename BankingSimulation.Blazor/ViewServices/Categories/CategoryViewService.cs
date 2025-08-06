using BankingSimulation.Blazor.Models.Categories;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.Blazor.ViewServices.Categories
{
    public class CategoryViewService(ICategoryProcessingService categoryProcessingService) : ICategoryViewService
    {
        public IEnumerable<CategoryViewModel> GetCategories()
            => categoryProcessingService.GetAll()
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Keywords = c.Keywords.OrderBy(r => r.Keyword).Select(r => r.Keyword),
                    EditUrl = "/categories/" + c.Id
                }).ToList();

        public async Task DeleteCategoryAsync(Guid categoryId)
        {
            var category = categoryProcessingService.GetAll()
                .FirstOrDefault(c => c.Id == categoryId);

            if (category != null)
                await categoryProcessingService.DeleteAsync(category);
        }
    }
}
