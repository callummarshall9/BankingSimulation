using BankingSimulation.Blazor.Models.Categories;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.Blazor.ViewServices.Categorys
{
    public class EditCategoryViewService(ICategoryProcessingService categoryProcessingService,
        ICategoryKeywordsProcessingService userCategoryProcessingService) : IEditCategoryViewService
    {
        public EditCategoryViewModel GetCategory(Guid categoryId)
        {
            var result = categoryProcessingService.GetAll()
                .Where(r => r.Id == categoryId)
                .Select(c => new EditCategoryViewModel
                {
                    Id = c.Id,
                    RoleId = c.RoleId,
                    Name = c.Name,
                    Description = c.Description,
                    Keywords = c.Keywords.OrderBy(ur => ur.Keyword).Select(ur => ur.Keyword).ToList()
                })
                .FirstOrDefault();

            if (result is null)
            {
                throw new InvalidOperationException($"Category with ID {categoryId} not found.");
            }

            return result;
        }

        public async Task<EditCategoryViewModel> UpdateCategoryAsync(EditCategoryViewModel editCategoryViewModel)
        {
            var updatedCategory = await categoryProcessingService.UpdateAsync(new Data.Models.Category
            {
                Id = editCategoryViewModel.Id,
                RoleId = editCategoryViewModel.RoleId,
                Name = editCategoryViewModel.Name,
                Description = editCategoryViewModel.Description
            });

            await MergeKeywordsAsync(editCategoryViewModel, updatedCategory);

            return GetCategory(editCategoryViewModel.Id);
        }

        private async Task MergeKeywordsAsync(EditCategoryViewModel editCategoryViewModel, Data.Models.Category updatedCategory)
        {
            var existingKeywords = userCategoryProcessingService
                .GetAll()
                .Where(c => c.CategoryId == editCategoryViewModel.Id)
                .ToList();

            foreach (var existingKeyword in existingKeywords)
            {
                var receivedEvent = editCategoryViewModel.Keywords.FirstOrDefault(ce =>
                    ce == existingKeyword.Keyword
                );

                if (receivedEvent == null)
                    await userCategoryProcessingService.DeleteAsync(existingKeyword);
            }

            foreach (var incomingKeyword in editCategoryViewModel.Keywords)
            {
                var existingEvent = existingKeywords.FirstOrDefault(ce =>
                    ce.Keyword == incomingKeyword
                );

                if (existingEvent == null)
                    await userCategoryProcessingService.AddAsync(new Data.Models.CategoryKeyword
                    {
                        CategoryId = updatedCategory.Id,
                        Keyword = incomingKeyword
                    });
            }
        }
    }
}
