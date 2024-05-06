using BankingSimulation.Data.Models;
using BankingSimulation.Services.Foundation;
using System.Security;

namespace BankingSimulation.Services.Processing
{
    public class CategoryKeywordsProcessingService(ICategoryKeywordFoundationService foundationService) : ICategoryKeywordsProcessingService
    {
        public async Task<CategoryKeyword> AddAsync(CategoryKeyword item)
        {
            bool categoryExists = foundationService.GetAll<Category>()
                .Any(c => c.Id == item.CategoryId);

            if (!categoryExists)
                throw new SecurityException("Access Denied!");

            var result = await foundationService.AddAsync(new CategoryKeyword { CategoryId = item.CategoryId, Keyword = item.Keyword });

            await foundationService.UpdateTransactionsForCategory(item.CategoryId);

            return result;
        }

        public async Task DeleteAsync(CategoryKeyword item)
        {
            Guid dbCategoryId = foundationService.GetAll<CategoryKeyword>()
                .Where(c => c.Id == item.Id && c.Category != null)
                .Select(c => c.CategoryId)
                .FirstOrDefault();

            if (dbCategoryId == default)
                throw new SecurityException("Access Denied!");

            await foundationService.DeleteAsync(new CategoryKeyword { Id = item.Id });

            await foundationService.UpdateTransactionsForCategory(dbCategoryId);
        }

        public IQueryable<CategoryKeyword> GetAll()
            => foundationService.GetAll<CategoryKeyword>();

        public async Task<CategoryKeyword> UpdateAsync(CategoryKeyword item)
        {
            Guid dbCategoryId = foundationService.GetAll<CategoryKeyword>()
                .Where(c => c.Id == item.Id && c.Category != null)
                .Select(c => c.CategoryId)
                .FirstOrDefault();

            if (dbCategoryId == default)
                throw new SecurityException("Access Denied!");

            if (item.CategoryId != dbCategoryId)
                throw new InvalidOperationException("Cannot reassign keywords to new category");

            var result = await foundationService.UpdateAsync(new CategoryKeyword
            {
                Id = item.Id,
                Keyword = item.Keyword,
                CategoryId = item.CategoryId
            });

            await foundationService.UpdateTransactionsForCategory(item.CategoryId);

            return result;
        }

        public Task UpdateTransactionsForCategoryKeyword(Guid categoryId)
            => foundationService.UpdateTransactionsForCategory(categoryId);
    }
}
